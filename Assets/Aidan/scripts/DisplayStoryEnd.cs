using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayStoryEnd : MonoBehaviour
{
    [SerializeField] TextAsset lucaStory;
    [SerializeField] TextAsset RoxyStory;

    [SerializeField, Range(0, 1)] float lucaAffection;

     
    public void StartStory(int character)
    {
        if (!DialogueManager.instance.StoryEnded()) return;

        float affection = 1f;
        DialogueManager.instance.StartDialogueKnotName(character == 1 ? lucaStory : RoxyStory, (CharacterName)character-1, affection > 0.5 ? "Good_End" : "Bad_End");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }


}
