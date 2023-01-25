using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InkParser : MonoBehaviour
{
    [SerializeField] private DialogueCoordinator coordinator;

    Regex rx = new Regex(@"\b(\w+)\s*:\s*(\w+)");
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
        print("Ink Step: " + tags.Count + " tags found");

        foreach(string tag in tags)
        {
            Match m = rx.Match(tag);
            if(!m.Success)
            {
                Debug.LogWarning("Something went wrong, returning");
                return;
            }

            // m.groups [0] is the entire match
            Group section = m.Groups[1];
            Group modifier = m.Groups[2];

            if (section.Value == "Speaker")
            {
                print("Speaker Change: " + modifier.Value);
                prevSpeaker = modifier.Value;
                coordinator.ChangeSpeaker(modifier.Value);
            }
            else if (section.Value == "Image")
            {
                print("Image Change: " + modifier.Value);
                string path = "ChatPortraits/" + prevSpeaker + "/" + modifier.Value;
                coordinator.ChangeImage(path);
            }
            else
            {
                Debug.LogWarning(section.Value + ": Invalid Section");
            }
        }
    }
}
