using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {
    
    [SerializeField] private DialogueCoordinator coordinator;
    DialogueManager dMan;

    private void Start()
    {
        dMan = DialogueManager.instance;
    }

    void Update()
    {
        if (!dMan || !dMan.speakingCharacter) return;
    }

    public bool IsDialogueActive()
    {
        return !coordinator.StoryEnded();
    }

    public void StartDialogue(TextAsset inkStory, CharacterName character)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(character);
    }

    public void StartDialogue(TextAsset inkStory, CharacterName character, int progress)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(progress, character);
    }

    public void StartDialogue(TextAsset inkStory, CharacterName character, string knotName)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(knotName, character);
    }

    public void StopDialogue()
    {
        coordinator.StopDialogue();
    }

    public bool StoryEnded()
    {
        return coordinator.StoryEnded();
    }
}
