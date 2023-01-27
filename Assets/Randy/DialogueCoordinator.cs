using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueCoordinator : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image uiPanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image leftSpeakerImage;

    [Header("Ink Integration")]
    [SerializeField] private InkParser parser;
    [SerializeField] private Story currentStory;

    private Coroutine writeDialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadCharacterStory(TextAsset inkStory)
    {
        currentStory = new Story(inkStory.text);
    }

    public void StartDialogue()
    {
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(int progress)
    {
        currentStory.variablesState["CurrentStoryState"] = progress;
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(string knotName)
    {
        currentStory.ChoosePathString(knotName);
        writeDialogue = StartCoroutine(WriteDialogue());
    }

    private IEnumerator WriteDialogue()
    {
        string lineText;
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
        // loop through each letter in text and add it to text
        while (currentStory.canContinue)
        {
            lineText = currentStory.Continue();
            parser.ParseTags(currentStory.currentTags);
            for (int i = 0; i < lineText.Length; i++)
            {
                int intAlpha = (int)Mathf.Round(panelGroup.alpha*255);
                dialogueText.text += "<alpha=#" + intAlpha.ToString("X2") + ">" + lineText[i];

                // play sound (outside function)

                yield return new WaitForSeconds(DialogueManager.instance.GetTextRenderDelay());
            }
            yield return new WaitForSeconds(DialogueManager.instance.GetNextLineDelay());
            ClearDialogueText();
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
        leftSpeakerImage.gameObject.SetActive(false);
    }

    public IEnumerator PanelFadeout()
    {
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
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
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
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

    public void StopDialogue()
    {
        if (writeDialogue != null)
            StopCoroutine(writeDialogue);
        ClearDialogueText();
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
}
