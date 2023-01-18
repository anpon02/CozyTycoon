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
    [SerializeField] private float minFadeoutRate;
    [SerializeField] private float maxFadeoutRate;
    [SerializeField] private float fadeinRate;


    [Header("Debug Tools")]
    [SerializeField] bool showDebugInfo;
    [ Range(0f, 100f)] public float playerDistance;

    
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

    public float GetMinFadeoutThreshold()
    {
        return minFadeoutThreshold;
    }
    
}
