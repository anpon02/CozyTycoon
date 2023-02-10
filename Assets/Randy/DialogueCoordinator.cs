using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueCoordinator : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image dialoguePanel, speakerPortrait;
    [SerializeField] TextMeshProUGUI mainText, speakerName;

    [Header("Ink Integration")]
    [SerializeField] InkParser parser;
    [SerializeField] Story currentStory;

    [Header("Choices")]
    [SerializeField] GameObject choiceParent;
    [SerializeField] TextMeshProUGUI choicePrompt, button1, button2, button3;
    Choice choice1, choice2, choice3;
    DialogueManager dMan;

    [Header("Fade")]
    public float fadeModifier = 0.5f;
    public float maxDist;
    public float minAlpha = 0.1f;
    CanvasGroup panelGroup;

    private Coroutine writeDialogue;

    void Start()
    {
        panelGroup = dialoguePanel.transform.parent.GetOrAddComponent<CanvasGroup>();
        dMan = DialogueManager.instance;
        dialoguePanel.gameObject.SetActive(false);
    }

    public void LoadCharacterStory(TextAsset inkStory)
    {
        currentStory = new Story(inkStory.text);
    }

    public Story GetCharacterStory()
    {
        return currentStory;
    }

    public void StartDialogue(int progress, CharacterName character)
    {
        currentStory.variablesState["CurrentStoryState"] = progress;
        dialoguePanel.gameObject.SetActive(true);
        mainText.gameObject.SetActive(true);
        StartDialogue(character);
    }

    public void StartDialogue(string knotName, CharacterName character)
    {
        currentStory.ChoosePathString(knotName);
        dialoguePanel.gameObject.SetActive(true);
        StartDialogue(character);
    }

    public void StartDialogue(CharacterName character)
    {
        var data = dMan.GetSpeakerData(character);
        speakerPortrait.sprite = data.portrait;
        speakerName.text = data.name;
        StartDialogue();
    }

    void StartDialogue()
    {
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    public void StopDialogue()
    {
        StopAllCoroutines();
        ClearAll();
        dialoguePanel.gameObject.SetActive(false);
        dMan.OnDialogueEnd.Invoke();
    }

    private void Update()
    {
        if (dMan.speakingCharacter) SetGroupAlpha();
    }

    private IEnumerator WriteDialogue()
    {
        string lineText = default(string);

        while (currentStory.canContinue)
        {
            lineText = currentStory.Continue().Trim();
            parser.ParseTags(currentStory.currentTags);
            for (int i = 0; i < lineText.Length; i++)
            {
                mainText.text += lineText[i];

                AudioManager.instance.PlaySound(dMan.GetSpeakerData(dMan.lastSpeaker).speakerSoundID);
                yield return new WaitForSeconds(dMan.GetTextRenderDelay() / dMan.GetTextRenderModifier());
            }

            yield return new WaitForSeconds(dMan.GetNextLineDelay() * dMan.GetLineDelayModifier());
            ClearDialogueText();
            dMan.ResetModifiers();
        }

        DisplayChoices(lineText);
        if (currentStory.currentChoices.Count == 0) StopDialogue();
    }

    void SetGroupAlpha()
    {
        float dist = dMan.SpeakerDistance;
        dist = Mathf.Min(dist * fadeModifier, maxDist);

        panelGroup.alpha = 1 - Mathf.Max(dist / maxDist, minAlpha);
    }

    void DisplayChoices(string prompt)
    {
        ResetChoiceButtons();

        int choiceCount = currentStory.currentChoices.Count;
        if (choiceCount > 0) SetupAndEnableButton(0, button1);
        if (choiceCount > 1) SetupAndEnableButton(1, button2);
        if (choiceCount > 2) SetupAndEnableButton(2, button3);

        bool choices = currentStory.currentChoices.Count > 0;
        choicePrompt.text = prompt;
        choicePrompt.gameObject.SetActive(choices);
        choiceParent.SetActive(choices);
    }

    void ResetChoiceButtons() {
        ResetChoiceButton(button1);
        ResetChoiceButton(button2);
        ResetChoiceButton(button3);
        choice1 = choice2 = choice3 = null;
    }

    void ResetChoiceButton(TextMeshProUGUI buttonText)
    {
        buttonText.transform.parent.gameObject.SetActive(false);
        buttonText.text = "";
    }

    void SetupAndEnableButton(int index, TextMeshProUGUI button)
    {
        var choice = currentStory.currentChoices[index];
        if (index == 0) choice1 = choice;
        if (index == 1) choice2 = choice;
        if (index == 2) choice3 = choice;

        button.text = choice.text.Trim();
        button.transform.parent.gameObject.SetActive(true);
    }

    public void MakeChoice(int num)
    {
        if (num == 1 && choice1 != null) MakeChoice(choice1);
        if (num == 2 && choice2 != null) MakeChoice(choice2);
        if (num == 3 && choice3 != null) MakeChoice(choice3);
    }

    void MakeChoice(Choice choice)
    {
        choiceParent.SetActive(false);
        currentStory.ChooseChoiceIndex(choice.index);
        currentStory.Continue();
        StartCoroutine(WriteDialogue());
    }

    private void ClearDialogueText()
    {
        mainText.text = string.Empty;
    }

    private void ClearAll()
    {

        speakerName.text = string.Empty;
        mainText.text= string.Empty;
        speakerPortrait.sprite = null;
    }

    public Image GetDialoguePanel()
    {
        return dialoguePanel;
    }

    public void ChangeSpeaker(string name)
    {
        speakerName.text = name;
    }

    public void ChangeImage(string path)
    {
        return;
        speakerPortrait.sprite = Resources.Load<Sprite>(path);
    }

    public bool StoryEnded()
    {
        return !dialoguePanel.IsActive();
    }
}
