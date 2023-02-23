using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
public class DialogueController : MonoBehaviour {
    [Header("Ink Integration")]
    [SerializeField] InkParser parser;
    [SerializeField] private DialogueCoordinator coordinator;
    DialogueManager dMan;
    [HideInInspector] public Choice choice1, choice2, choice3;

    [Header("Sounds")]
    [SerializeField] int uiclickSound;

    private void Start()
    {
        dMan = DialogueManager.instance;
    }

    void Update()
    {
        if (!dMan || !dMan.speakingCharacter) return;
    }

    IEnumerator WriteDialogue()
    {
        Story story = dMan.currentStory;
        string lineText = default(string);
        while (story.canContinue)
        {
            lineText = story.Continue().Trim();
            parser.ParseTags(story.currentTags);
            bool notable = dMan.allowNotes;
            // Write and display dialogue line thorugh coordinator
            yield return StartCoroutine(coordinator.DisplayText(lineText, notable));
            yield return new WaitForSeconds(dMan.GetNextLineDelay() * dMan.GetLineDelayModifier());
            
            coordinator.ClearDialogueText();
            dMan.ResetModifiers();
        }

        coordinator.DisplayChoices(lineText);
        if (story.currentChoices.Count == 0) StopDialogue();
    }

    public void MakeChoice(int num)
    {
        if (num == 1 && choice1 != null) MakeChoice(choice1);
        if (num == 2 && choice2 != null) MakeChoice(choice2);
        if (num == 3 && choice3 != null) MakeChoice(choice3);
    }


    void MakeChoice(Choice choice)
    {
        coordinator.choiceParent.SetActive(false);
        Story story = dMan.currentStory;
        story.ChooseChoiceIndex(choice.index);
        story.Continue();
        AudioManager.instance.PlaySound(uiclickSound, gameObject);
        StartCoroutine(WriteDialogue());
        
    }

    public bool IsDialogueActive()
    {
        return !coordinator.StoryEnded();
    }

    public void StartDialogue(CharacterName character)
    {
        coordinator.ShowDialoguePanel();
        coordinator.SetSpeakerData(character);
        StartCoroutine(WriteDialogue());
    }

    public void StopDialogue()
    {
        StopAllCoroutines();
        coordinator.StopAllCoroutines();
        coordinator.ClearAll();
        coordinator.HideDialoguePanel();
        dMan.OnDialogueEnd.Invoke();
    }

    public bool StoryEnded()
    {
        return coordinator.StoryEnded();
    }
}
