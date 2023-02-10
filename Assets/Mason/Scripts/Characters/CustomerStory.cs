using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStory : MonoBehaviour
{
    public CharacterName characterName;
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
        if (!DialogueManager.instance) return;

        DialogueManager.instance.speakingCharacter = gameObject;
        DialogueManager.instance.StartDialogue(inkStory, characterName, storyPhaseNum);
        storySaid = true;
        NextStoryPhase();
    }

    public bool GetStorySaid() {
        return storySaid;
    }

    public void SetStorySaid(bool said) {
        storySaid = said;
    }
}
