using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStory : MonoBehaviour
{
    [SerializeField] private TextAsset inkStory;
    private int storyPhaseNum;
    private bool storySaid;

    private void Awake() {
        storyPhaseNum = 0;
        storySaid = false;
    }

    public int GetStoryPhaseNum() {
        return storyPhaseNum;
    }

    public void NextStoryPhase() {
        storyPhaseNum++;
    }

    public TextAsset GetInkStory() {
        return inkStory;
    }

    public void StartStory() {
        if(DialogueManager.instance) {
            DialogueManager.instance.StartDialogue(inkStory, storyPhaseNum);
            DialogueManager.instance.SetSpeakingCharacter(gameObject);
            storySaid = true;
            NextStoryPhase();
        }
    }

    public bool GetStorySaid() {
        return storySaid;
    }

    public void SetStorySaid(bool said) {
        storySaid = said;
    }
}
