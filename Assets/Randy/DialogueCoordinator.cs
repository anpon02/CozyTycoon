using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueCoordinator : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image dialoguePanel;
    [SerializeField] private Image choicePanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image leftSpeakerImage;

    [Header("Ink Integration")]
    [SerializeField] private InkParser parser;
    [SerializeField] private Story currentStory;
    [SerializeField, Tooltip("Used to display text for choices.")] private Button buttonPrefab;
    [SerializeField, Tooltip("Used to display text for choices.")] private TextMeshProUGUI textPrefab;

    private Coroutine writeDialogue;

    void Start()
    {
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


    public void StartDialogue()
    {
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(int progress)
    {
        currentStory.variablesState["CurrentStoryState"] = progress;
        dialoguePanel.gameObject.SetActive(true);
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(string knotName)
    {
        currentStory.ChoosePathString(knotName);
        dialoguePanel.gameObject.SetActive(true);
        writeDialogue = StartCoroutine(WriteDialogue());
    }
    public void StopDialogue()
    {
        if (writeDialogue != null)
            StopCoroutine(writeDialogue);
        ClearAll();
    }

    private IEnumerator WriteDialogue()
    {
        string lineText = default(string);
        CanvasGroup panelGroup = dialoguePanel.GetComponent<CanvasGroup>();
        panelGroup.alpha = 1f;
        StartCoroutine(ApplyPanelFade());
        // loop through each letter in text and add it to text
        while (currentStory.canContinue)
        {
            lineText = currentStory.Continue().Trim();
            parser.ParseTags(currentStory.currentTags);
            for (int i = 0; i < lineText.Length; i++)
            {
                int intAlpha = (int)Mathf.Round(panelGroup.alpha * 255);
                dialogueText.text += "<alpha=#" + intAlpha.ToString("X2") + ">" + lineText[i];

                // play sound
                AudioManager.instance.PlaySound(DialogueManager.instance.GetCharacterVoiceID());

                yield return new WaitForSeconds(DialogueManager.instance.GetTextRenderDelay() / DialogueManager.instance.GetTextRenderModifier());
            }
            yield return new WaitForSeconds(DialogueManager.instance.GetNextLineDelay() * DialogueManager.instance.GetLineDelayModifier());
            ClearDialogueText();
            DialogueManager.instance.ResetModifiers();
        }

        // Display all the choices, if there are any
        if (currentStory.currentChoices.Count > 0)
        {
            TextMeshProUGUI promptText = Instantiate(textPrefab) as TextMeshProUGUI;
            promptText.transform.SetParent(choicePanel.transform, false);
            promptText.text = lineText;
            for (int i = 0; i < currentStory.currentChoices.Count; i++)
            {
                Choice choice = currentStory.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim());
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
                choicePanel.gameObject.SetActive(true);
            }
        }
        dialoguePanel.gameObject.SetActive(false);
    }

    // From Ink's example script. modified
    Button CreateChoiceView(string text)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(choicePanel.transform, false);

        // Gets the text from the button prefab
        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;

        // Make the button expand to fit the text
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // From Ink's example script, modified
    void OnClickChoiceButton(Choice choice)
    {
        currentStory.ChooseChoiceIndex(choice.index);
        // Skip the text given by selecting the choice
        currentStory.Continue();
        dialoguePanel.gameObject.SetActive(true);
        ClearChoices();
        choicePanel.gameObject.SetActive(false);
        StartCoroutine(WriteDialogue());
    }

    void ClearChoices()
    {
        int childCount = choicePanel.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(choicePanel.transform.GetChild(i).gameObject);
        }
    }

    private void ClearDialogueText()
    {
        dialogueText.text = string.Empty;
    }

    private void ClearAll()
    {
        speakerText.text = string.Empty;
        dialogueText.text= string.Empty;
        leftSpeakerImage.sprite = null;
        ClearChoices();
        choicePanel.gameObject.SetActive(false);
    }

    public IEnumerator PanelFadeout()
    {
        CanvasGroup panelGroup = dialoguePanel.GetComponent<CanvasGroup>();
        while (DialogueManager.instance.GetPlayerDistance() > DialogueManager.instance.GetMinFadeoutThreshold())
        {
            if (panelGroup.alpha > 0.001f)
            {
                float fadeoutValue = 1/DialogueManager.instance.GetFadeoutRate() * DialogueManager.instance.GetFadeoutRateMultiplier(DialogueManager.instance.GetPlayerDistance());
                panelGroup.alpha -=  fadeoutValue * Time.deltaTime;
                Mathf.Clamp(panelGroup.alpha, 0f, 1f);
            }
            yield return null;
        }
    }

    public IEnumerator PanelFadein()
    {
        CanvasGroup panelGroup = dialoguePanel.GetComponent<CanvasGroup>();
        while (DialogueManager.instance.GetPlayerDistance() < DialogueManager.instance.GetMinFadeoutThreshold())
        {
            if (panelGroup.alpha < 0.999f)
            {
                panelGroup.alpha += 1/DialogueManager.instance.GetFadeinRate() * Time.deltaTime;
                Mathf.Clamp(panelGroup.alpha, 0f, 1f);
            }
            yield return null;
        }
    }

    public IEnumerator ApplyPanelFade()
    {
        CanvasGroup panelGroup = dialoguePanel.GetComponent<CanvasGroup>();
        while(dialoguePanel.IsActive())
        {
            panelGroup.alpha += 1 / DialogueManager.instance.GetFadeRate() * Time.deltaTime;
            Mathf.Clamp(panelGroup.alpha, 0.1f, 1f);
            yield return null;
        }
        print("end fade");
    }    

    public Image GetDialoguePanel()
    {
        return dialoguePanel;
    }

    public void ChangeSpeaker(string name)
    {
        speakerText.text = name;
    }

    public void ChangeImage(string path)
    {
        print(path);
        leftSpeakerImage.sprite = Resources.Load<Sprite>(path);
    }

    public bool StoryEnded()
    {
        return !dialoguePanel.IsActive();
    }
}
