using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Ink.Runtime;
using System;
using static AudioManager;
using UnityEngine.Events;

public class InkParser : MonoBehaviour
{
    [SerializeField] private DialogueCoordinator coordinator;

    Regex rx = new Regex(@"\b(\w+)(?:$|\s*:\s*(\S+))");
    private string prevSpeaker;
    private string prevImage;
    DialogueManager dMan;

    /*
    private void Start()
    {
        List<string> list = new List<string>();
        list.Add("Speaker: Bob");
        list.Add("Speaker: Jane");
        list.Add("Image: angry");
        ParseTags(list);
    }
    */
    private void Start()
    {
        dMan = DialogueManager.instance;
    }

    public void ParseTags(List<string> tags)
    {
        foreach (string tag in tags) ParseTag(tag);
    }

    void ParseTag(string tag)
    {
        Group section = default(Group);
        Group modifier = SetupTag(out section, tag);
        
        if (modifier == null || section == null) {
            Debug.LogWarning("modifier or section NULL");
            return;
        }

        switch (section.Value) {
            case "Voice":
                //typeParseCall<int>(modifier, dMan.SetCharacterVoiceID);
                return;
            case "Textspeed":
                typeParseCall<float>(modifier, dMan.SetTextRenderModifier);
                return;
            case "Linedelay":
                typeParseCall<float>(modifier, dMan.SetLineDelayModifier);
                return;
            case "ForceVisible":
                //typeParseCall<bool>(modifier, dMan.SetForcedVisibility);
                return;
            case "Speaker":
                IsSpeaker(modifier);
                return;
            case "Image":
                IsImage(modifier);
                return;
            case "Finished":
                IsFinished(modifier);
                return;
            case "CheckDistance":
                IsCheckDistance(modifier);
                return;
            case "Skippable":
                IsSkippable(modifier);
                return;
        }
    }

    Group SetupTag(out Group section, string tag)
    {
        section = null;
        Match m = rx.Match(tag);
        if (!m.Success) {
            Debug.LogWarning("Something went wrong, returning");
            return null;
        }

        section = m.Groups[1];
        Group modifier = default(Group);
        if (m.Groups.Count == 3) modifier = m.Groups[2];
        return modifier;
    }

    void typeParseCall<ParseType>(Group modifier,UnityAction<ParseType> call)
    {
        var data = default(ParseType);

        try {
            data = (ParseType)Convert.ChangeType(modifier.Value, typeof(ParseType));
        }
        catch (Exception) {
            Debug.LogWarning("Voice: Expected " + nameof(ParseType) + ", but received " + modifier.Value);
            return;
        }

        call.Invoke(data);
    }

    void IsSpeaker(Group modifier)
    {
        prevSpeaker = modifier.Value;
        coordinator.ChangeSpeaker(modifier.Value);
    }

    void IsImage(Group modifier)
    {
        string path = "ChatPortraits/" + modifier.Value;
        coordinator.ChangeImage(path);
    }    

    void IsFinished(Group modifier)
    {
        CustomerCoordinator story = dMan.speakingCharacter.GetComponent<CustomerCoordinator>();
        story.NextStoryPhase();
    }

    void IsCheckDistance(Group modifier)
    {
        Story story = dMan.currentStory;
        story.variablesState["Distance"] = dMan.SpeakerDistance;
    }

    void IsSkippable(Group modifier)
    {
        CanvasGroup panelGroup = coordinator.GetDialoguePanel().GetComponent<CanvasGroup>();
        if (panelGroup.alpha >= 0.001) return;
        dMan.currentStory.Continue();
    }
}

