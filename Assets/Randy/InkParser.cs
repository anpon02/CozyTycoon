using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Ink.Runtime;

public class InkParser : MonoBehaviour
{
    [SerializeField] private DialogueCoordinator coordinator;

    Regex rx = new Regex(@"\b(\w+)(?:$|\s*:\s*(\S+))");
    private string prevSpeaker;
    private string prevImage;

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
    public void ParseTags(List<string> tags)
    {
        //print("Ink Step: " + tags.Count + " tags found");

        foreach(string tag in tags)
        {
            Match m = rx.Match(tag);
            if(!m.Success)
            {
                Debug.LogWarning("Something went wrong, returning");
                return;
            }

            // m.groups [0] is the entire match
            //print(m.Groups[0].Value);
            Group section = m.Groups[1];
            Group modifier = default(Group);
            if (m.Groups.Count == 3)
                modifier = m.Groups[2];

            // Determine what to do for each RegEx match
            if (section.Value == "Speaker")
            {
                //print("Speaker Change: " + modifier.Value);
                prevSpeaker = modifier.Value;
                coordinator.ChangeSpeaker(modifier.Value);
            }
            else if (section.Value == "Image")
            {
                //print("Image Change: " + modifier.Value);
                string path = "ChatPortraits/" + modifier.Value;
                coordinator.ChangeImage(path);
            }
            else if (section.Value == "Voice")
            {
                int soundID;
                if (int.TryParse(modifier.Value, out soundID))
                {
                    DialogueManager.instance.SetCharacterVoiceID(soundID);
                }
                else
                {
                    Debug.LogWarning("Voice: Expected int, but received " + modifier.Value);
                }
            }
            else if (section.Value == "Textspeed")
            {
                float multiplier;
                if(float.TryParse(modifier.Value, out multiplier))
                {
                    DialogueManager.instance.SetTextRenderModifier(multiplier);
                }
                else
                {
                    Debug.LogWarning("Voice: Expected float, but received " + modifier.Value);
                }
            }
            else if (section.Value == "Linedelay")
            {
                float multiplier;
                if (float.TryParse(modifier.Value, out multiplier))
                {
                    DialogueManager.instance.SetLineDelayModifier(multiplier);
                }
                else
                {
                    Debug.LogWarning("Voice: Expected float, but received " + modifier.Value);
                }
            }
            else if (section.Value == "Finished")
            {
                CustomerStory story = DialogueManager.instance.GetSpeakingCharacter().GetComponent<CustomerStory>();
                story.NextStoryPhase();
            }
            else if (section.Value == "CheckDistance")
            {
                Story story = coordinator.GetCharacterStory();
                story.variablesState["Distance"] = DialogueManager.instance.GetPlayerDistance();
                print(DialogueManager.instance.GetPlayerDistance());
            }
            else if (section.Value == "ForceVisible")
            {
                bool state;
                if(bool.TryParse(modifier.Value.Trim(), out state))
                {
                    DialogueManager.instance.SetForcedVisibility(state);
                }
                else
                {
                    Debug.LogWarning("ForceVisible: Expected true/false, but received " + modifier.Value);
                }
            }
            else if (section.Value == "Skippable")
            {
                CanvasGroup panelGroup = coordinator.GetDialoguePanel().GetComponent<CanvasGroup>();
                if (panelGroup.alpha < 0.001)
                {
                    coordinator.GetCharacterStory().Continue();
                }
            }
            else
            {
                Debug.LogWarning(section.Value + ": Invalid Section");
            }
        }
    }
}
