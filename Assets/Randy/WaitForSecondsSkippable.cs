using System.Collections;
using UnityEngine;

public class WaitForSecondsSkippable : CustomYieldInstruction
{
    private float waitTime;
    private bool prevSkipped;
    private System.Func<bool> skipCondition;

    public WaitForSecondsSkippable(float time, bool prevSkip, System.Func<bool> skipCond)
    {
        waitTime = time + Time.time;
        prevSkipped = prevSkip;
        skipCondition = skipCond;
    }

    public override bool keepWaiting
    {
        get
        {
            if (prevSkipped) return !skipCondition();
            return Time.time < waitTime; 
        }
    }
}