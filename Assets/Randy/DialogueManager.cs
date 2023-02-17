using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Events;

public enum CharacterName { LUCA, ROXY, TRIPP, SALLY, FLORIAN, PHIL}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private void Awake()  { instance = this; }

    [System.Serializable]
    public class SpeakerData {
        [HideInInspector] public string name;
        public CharacterName characterName;
        public Sprite portrait;
        public int speakerSoundID;
    }
    
    [SerializeField] List<SpeakerData> speakers = new List<SpeakerData>();
    public DialogueController controller;
    [HideInInspector] public UnityEvent OnDialogueEnd = new UnityEvent();
    [HideInInspector] public CharacterName lastSpeaker;

    [Header("Text Speed")]
    [SerializeField] private float textRenderDelay;
    [SerializeField] private float nextLineDelay;
    // Modifier values for speeds. Please adjust these through Ink as they are reset regularly
    [SerializeField] private bool showTextModifiers;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float textRenderModifier = 1;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float lineDelayModifier = 1;
    

    [HideInInspector]public GameObject speakingCharacter;
    [HideInInspector] public Story currentStory { get; private set; }
    public float SpeakerDistance { get { return GetPlayerDistance(); } }

    public SpeakerData GetSpeakerData(CharacterName speakerName)
    {
        foreach (var s in speakers) if (s.characterName == speakerName) return s;
        return null;
    }

    public bool IsDialogueActive()
    {
        return controller.IsDialogueActive();
    }

    float GetPlayerDistance()
    {
        if (!speakingCharacter) return Mathf.Infinity;

        return Vector3.Distance(GameManager.instance.player.transform.position, speakingCharacter.transform.position);
    }

    #region Text Speed
    public float GetTextRenderDelay()
    {
        return textRenderDelay;
    }

    public float GetNextLineDelay()
    {
        return nextLineDelay;
    }

    public void SetTextRenderModifier(float m)
    {
        textRenderModifier = m;
    }

    public float GetTextRenderModifier()
    {
        return textRenderModifier;
    }

    public void SetLineDelayModifier(float m)
    {
        lineDelayModifier = m;
    }

    public float GetLineDelayModifier()
    {
        return lineDelayModifier;
    }

    public void ResetModifiers()
    {
        textRenderModifier = 1f;
        lineDelayModifier = 1f;
    }
    #endregion

    public bool StoryEnded()
    {
        return controller.StoryEnded();
    }

    public void StartDialogueMainStory(TextAsset inkStory, CharacterName character, int progress)
    {
        lastSpeaker = character;
        StopDialogue();
        currentStory = new Story(inkStory.text);
        currentStory.variablesState["CurrentStoryState"] = progress;
        controller.StartDialogue(character);
    }

    public void StartDialogueKnotName(TextAsset inkStory, CharacterName character, string knotName)
    {
        lastSpeaker = character;
        StopDialogue();
        currentStory = new Story(inkStory.text);
        currentStory.ChoosePathString(knotName);
        controller.StartDialogue(character);
    }

    public void StopDialogue()
    {
        controller.StopDialogue();
    }

}
