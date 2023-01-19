using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [Header("Text Speed")]
    [SerializeField] private float textRenderDelay;
    [SerializeField] private float nextLineDelay;

    [Header("Textbox Shenannigans")]
    [SerializeField] private float unintelligibleTextThreshold;
    [SerializeField] private float minFadeoutThreshold;
    [SerializeField] private float maxFadeoutThreshold;
    [SerializeField] private float FadeoutRate;
    [SerializeField] private float fadeinRate;


    [Header("Debug Tools (Will add ability to hide at some point)")]
    [SerializeField] private bool showDebugInfo;
    [ Range(0f, 100f)] public float playerDistance;
    [SerializeField] public bool isUnintelligible;
    [SerializeField] public bool isFadingOut;
    [SerializeField] public bool isFadingIn;
    

    
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

}
