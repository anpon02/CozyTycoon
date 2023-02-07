using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    // Note from Randy: This class is getting long. I will split this into two classes after the week of playesting
    public static DialogueManager instance;
    [SerializeField] DialogueController controller;

    [Header("Text Speed")]
    [SerializeField] private float textRenderDelay;
    [SerializeField] private float nextLineDelay;
    // Modifier values for speeds. Please adjust these through Ink as they are reset regularly
    [SerializeField] private bool showTextModifiers;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float textRenderModifier = 1;
    [SerializeField, ConditionalHide(nameof(showTextModifiers))] private float lineDelayModifier = 1;

    [Header("Textbox Shenannigans")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minFadeoutThreshold;
    [SerializeField] private float maxFadeoutThreshold;
    [SerializeField] private float FadeoutRate;
    [SerializeField] private float fadeinRate;
    private float currentFadeRate;


    [Header("Debug Tools (Will add ability to hide at some point)")]
    [SerializeField] private bool showDebugInfo;
    [ Range(0f, 100f), ConditionalHide(nameof(showDebugInfo))] private float playerDistance;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] private GameObject speakingCharacter;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] private int characterVoiceID;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] public bool isFadingOut;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] public bool isFadingIn;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] private bool visibilityForced;
    

    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
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

    #region Textbox Shenannagans
    public float GetFadeoutRate()
    {
        return FadeoutRate;
    }

    public float GetFadeinRate()
    {
        return fadeinRate;
    }

    public void SetFadeRate(float r)
    {
        currentFadeRate = r;
    }

    public float GetFadeRate()
    {
        return currentFadeRate;
    }

    public float GetMinFadeoutThreshold()
    {
        return minFadeoutThreshold;
    }

    public float GetMaxFadeoutThreshold()
    {
        return maxFadeoutThreshold;
    }

    public float GetFadeoutRateMultiplier(float distance)
    {
        return Mathf.Clamp(Mathf.InverseLerp( GetMaxFadeoutThreshold(), GetMinFadeoutThreshold(), distance), 0.25f, 1f);
    }

    public void TrackChareacterDistance(GameObject npc, float minDist, float maxDist)
    {
        this.speakingCharacter = npc;
        this.minFadeoutThreshold = minDist;
        this.maxFadeoutThreshold = maxDist;
    }

    public void SetMinFadeoutThreshold(float distance)
    {
        minFadeoutThreshold = distance;
    }

    public void SetMaxFadeoutThreshold(float distance)
    {
        maxFadeoutThreshold = distance;
    }

    public void SetForcedVisibility(bool vis)
    {
        visibilityForced = vis;
    }

    public bool GetForcedVisibility()
    {
        return visibilityForced;
    }

    public bool StoryEnded()
    {
        return controller.StoryEnded();
    }

    #endregion

    #region Outside access/ Helper functions
    public void StartDialogue(TextAsset inkStory)
    {
        StopDialogue();
        controller.StartDialogue(inkStory);
    }

    public void StartDialogue(TextAsset inkStory, int progress)
    {
        StopDialogue();
        controller.StartDialogue(inkStory, progress);
    }

    public void StartDialogue(TextAsset inkStory, string knotName)
    {
        StopDialogue();
        controller.StartDialogue(inkStory, knotName);
    }

    public void StopDialogue()
    {
        controller.StopDialogue();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject _player) {
        player = _player;
    }

    public GameObject GetSpeakingCharacter()
    {
        return speakingCharacter;
    }

    public void SetCharacterVoiceID(int ID)
    {
        characterVoiceID = ID;
    }

    public int GetCharacterVoiceID()
    {
        return characterVoiceID;
    }

    public void SetSpeakingCharacter(GameObject npc)
    {
        this.speakingCharacter = npc;
    }

    // Should only have 1 reference to it
    public void SetPlayerDistance(float dist) {
        playerDistance = dist;
    }

    public float GetPlayerDistance()
    {
        return playerDistance;
    }
    #endregion
}
