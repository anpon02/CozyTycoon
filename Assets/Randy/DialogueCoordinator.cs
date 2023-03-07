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
    [SerializeField] Image dialoguePanel, speakerPortrait, skipButton;
    [SerializeField] Button dialogueButton;
    [SerializeField] TextMeshProUGUI mainText, speakerName;

    [Header("Choices")]
    public GameObject choiceParent;
    [SerializeField] TextMeshProUGUI choicePrompt, button1, button2, button3;
    DialogueManager dMan;

    [Header("Fade")]
    public float fadeModifier = 0.5f;
    public float maxDist;
    public float minAlpha = 0.1f;
    CanvasGroup panelGroup;

    void Start()
    {
        panelGroup = dialoguePanel.transform.parent.GetOrAddComponent<CanvasGroup>();
        dMan = DialogueManager.instance;
        dialoguePanel.gameObject.SetActive(false);
    }

    public void ShowDialoguePanel()
    {
        dialoguePanel.gameObject.SetActive(true);
        mainText.gameObject.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        dialoguePanel.gameObject.SetActive(false);
    }

    public void SetSpeakerData(CharacterName character)
    {
        var data = dMan.GetSpeakerData(character);
        speakerPortrait.sprite = data.portrait;
        speakerName.text = data.name;
    }

    private void Update()
    {
        if (dMan.speakingCharacter) SetGroupAlpha();
        if (dMan.allowNotes != dialogueButton.interactable) dialogueButton.interactable = dMan.allowNotes;
        if (dMan.convoSkippable != skipButton.IsActive()) skipButton.gameObject.SetActive(dMan.convoSkippable);
    }

    public IEnumerator DisplayText(string text, bool notable)
    {
        if(notable)
        {
            mainText.fontStyle = FontStyles.Italic;
            mainText.color = dMan.notableTextColor;
        }
        else
        {
            mainText.fontStyle = FontStyles.Normal;
            mainText.color = Color.black;
        }
        for (int i = 0; i < text.Length; i++)
        {
            if(dMan.skipPrint)
            {
                mainText.text = text;
                break;
            }
            mainText.text += text[i];
            if (mainText.text.Length % 2 == 1) AudioManager.instance.PlaySound(dMan.GetSpeakerData(dMan.lastSpeaker).speakerSoundID);
            yield return new WaitForSeconds(dMan.GetTextRenderDelay() / dMan.GetTextRenderModifier());
        }
        dMan.lineDone = true;
    }

    void SetGroupAlpha()
    {
        float dist = dMan.SpeakerDistance;
        dist = Mathf.Min(dist * fadeModifier, maxDist);

        panelGroup.alpha = 1 - Mathf.Min(dist / maxDist, (1-minAlpha));
    }

    public void DisplayChoices(string prompt)
    {
        ResetChoiceButtons();
        Story story = dMan.currentStory;

        int choiceCount = story.currentChoices.Count;
        if (choiceCount > 0) SetupAndEnableButton(0, button1);
        if (choiceCount > 1) SetupAndEnableButton(1, button2);
        if (choiceCount > 2) SetupAndEnableButton(2, button3);

        bool choices = story.currentChoices.Count > 0;
        choicePrompt.text = prompt;
        choicePrompt.gameObject.SetActive(choices);
        choiceParent.SetActive(choices);
    }

    void ResetChoiceButtons() {
        ResetChoiceButton(button1);
        ResetChoiceButton(button2);
        ResetChoiceButton(button3);
        dMan.controller.choice1 = dMan.controller.choice2 = dMan.controller.choice3 = null;
    }

    void ResetChoiceButton(TextMeshProUGUI buttonText)
    {
        buttonText.transform.parent.gameObject.SetActive(false);
        buttonText.text = "";
    }

    void SetupAndEnableButton(int index, TextMeshProUGUI button)
    {
        var choice = dMan.currentStory.currentChoices[index];
        if (index == 0) dMan.controller.choice1 = choice;
        if (index == 1) dMan.controller.choice2 = choice;
        if (index == 2) dMan.controller.choice3 = choice;

        button.text = choice.text.Trim();
        button.transform.parent.gameObject.SetActive(true);
    }

    public void ClearDialogueText()
    {
        mainText.text = string.Empty;
    }

    public void ClearAll()
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
        //speakerPortrait.sprite = Resources.Load<Sprite>(path);
    }

    public bool StoryEnded()
    {
        return !dialoguePanel.IsActive();
    }
}
