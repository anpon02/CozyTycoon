using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField] DialogueController controller;

    [Header("Text Speed")]
    [SerializeField] private float textRenderDelay;
    [SerializeField] private float nextLineDelay;

    [Header("Textbox Shenannigans")]
    [SerializeField] private GameObject player;
    // maybe clean up line below
    [SerializeField] private float unintelligibleTextThreshold;
    [SerializeField] private float minFadeoutThreshold;
    [SerializeField] private float maxFadeoutThreshold;
    [SerializeField] private float FadeoutRate;
    [SerializeField] private float fadeinRate;


    [Header("Debug Tools (Will add ability to hide at some point)")]
    [SerializeField] private bool showDebugInfo;
    [ Range(0f, 100f), ConditionalHide(nameof(showDebugInfo))] public float playerDistance;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] private GameObject speakingCharacter;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] public bool isUnintelligible;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] public bool isFadingOut;
    [SerializeField, ConditionalHide(nameof(showDebugInfo))] public bool isFadingIn;
    

    
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetTextRenderDelay()
    {
        return textRenderDelay;
    }

    public float GetNextLineDelay()
    {
        return nextLineDelay;
    }

    public float GetFadeoutRate()
    {
        return FadeoutRate;
    }

    public float GetFadeinRate()
    {
        return fadeinRate;
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
        return Mathf.InverseLerp(GetMinFadeoutThreshold(), GetMaxFadeoutThreshold(), distance);
    }

    public void TrackChareacterDistance(GameObject npc)
    {
        this.speakingCharacter = npc;
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

    public void StartDialogue(TextAsset inkStory)
    {
        controller.StartDialogue(inkStory);
    }

    public void StartDialogue(TextAsset inkStory, int progress)
    {
        controller.StartDialogue(inkStory, progress);
    }

    public void StartDialogue(TextAsset inkStory, string knotName)
    {
        controller.StartDialogue(inkStory, knotName);
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
