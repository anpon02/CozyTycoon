using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueCoordinator : MonoBehaviour
{
    [SerializeField] private DialogueController controller;

    [Header("UI Elements")]
    [SerializeField] private Image uiPanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image leftSpeakerImage;
    [SerializeField] private Image rightSpeakerImage;

    [Header("Ink Integration")]
    [SerializeField] private Story currentStory;

    [Header("Testing, remove later")]
    [SerializeField] private TextAsset inkStory;

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
        StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(int progress)
    {
        currentStory.variablesState["CurrentStoryState"] = progress;
        StartCoroutine(WriteDialogue());
    }

    public void StartDialogue(string knotName)
    {
        currentStory.ChoosePathString(knotName);
        StartCoroutine(WriteDialogue());
    }

    private IEnumerator WriteDialogue()
    {
        string lineText;
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
        // loop through each letter in text and add it to text
        while (currentStory.canContinue)
        {
            lineText = currentStory.Continue();
            for (int i = 0; i < lineText.Length; i++)
            {
                int intAlpha = (int)Mathf.Round(panelGroup.alpha*255);
                // if player in fadeoutThreshold
                    // place text as normal
                    dialogueText.text += "<alpha=#" + intAlpha.ToString("X2") + ">" + lineText[i];
                // else output some jarbled characters from some pool (offload to outside function)

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
        rightSpeakerImage.sprite = null;
        rightSpeakerImage.gameObject.SetActive(false);
    }

    public IEnumerator PanelFadeout()
    {
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
        while (DialogueManager.instance.playerDistance > DialogueManager.instance.GetMinFadeoutThreshold())
        {
            if (panelGroup.alpha > 0.001f)
            {
                panelGroup.alpha -=  DialogueManager.instance.GetFadeoutRate() * DialogueManager.instance.GetFadeoutRateMultiplier(DialogueManager.instance.playerDistance);
                Mathf.Clamp(panelGroup.alpha, 0f, 1f);
            }
            yield return null;
        }
    }

    public IEnumerator PanelFadein()
    {
        CanvasGroup panelGroup = uiPanel.GetComponent<CanvasGroup>();
        while (DialogueManager.instance.playerDistance < DialogueManager.instance.GetMinFadeoutThreshold())
        {
            if (panelGroup.alpha < 0.999f)
            {
                panelGroup.alpha += DialogueManager.instance.GetFadeinRate();
                Mathf.Clamp(panelGroup.alpha, 0f, 1f);
            }
            yield return null;
        }
    }


}
