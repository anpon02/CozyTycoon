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
        public Sprite lilPortrait;
        public int speakerSoundID;
        public bool disabled;
    }
    
    [SerializeField] List<SpeakerData> speakers = new List<SpeakerData>();
    public DialogueController controller;
    [HideInInspector] public UnityEvent OnDialogueEnd = new UnityEvent();
    [HideInInspector] public CharacterName lastSpeaker;
    [HideInInspector] public GameObject speakingCharacter;
    [HideInInspector] public Story currentStory { get; private set; }

    [Header("Text Speed")]
    [SerializeField] private float textRenderDelay;
    [SerializeField] private float nextLineDelay;
    // Modifier values for speeds. Please adjust these through Ink as they are reset regularly
    [SerializeField] private bool showTextModifiers;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float textRenderModifier = 1;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float lineDelayModifier = 1;
    [HideInInspector] public bool ConvoStarted;
    [HideInInspector] public bool convoSkippable;
    [HideInInspector] public bool lineDone;
    [HideInInspector] public bool skipPrint;
    [HideInInspector] public bool skipLine;

    [Header("Notetaking")]
    [HideInInspector] public bool allowNotes;
    public Color notableTextColor;

    private void OnValidate() {
        foreach (var s in speakers) s.name = s.characterName.ToString();
    }

    public float SpeakerDistance { get { return GetPlayerDistance(); } }

    private void Update()
    {
        if (speakingCharacter != null) ConvoStarted = true;
    }

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
        allowNotes = false;
        lineDone = false;
        skipPrint = false;
        skipLine = false;
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
        try { currentStory.variablesState["CurrentStoryState"] = progress; }
        catch (System.Exception e) {}
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

    public void TakeNotes()
    {
        if (!allowNotes) return;
        allowNotes = false;
        string note = currentStory.state.currentText.Trim();
        AudioManager.instance.PlaySound(7);
        GameManager.instance.notebook.RecordInfo(note, lastSpeaker);
    }

    public void DisableCharacterStory(CharacterName name)
    {
        SpeakerData character = speakers.Find(s => s.characterName == name);
        if (character == default(SpeakerData))
        {
            Debug.LogWarning("DisableCharacterStory: " + name + " not found");
            return;
        }
        character.disabled = true;
    }

    public bool StoryDisabled(CharacterName name)
    {
        return speakers.Find(s => s.characterName == name).disabled;
    }

    public void SetTextSkip(bool state)
    {
        convoSkippable = state;
    }

    public void Skip()
    {
        if(!skipPrint && lineDone)
        {
            skipPrint = true;
            return;
        }
        skipLine = true;
    }
}
