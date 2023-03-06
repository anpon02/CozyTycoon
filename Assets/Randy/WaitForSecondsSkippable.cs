using System.Collections;
using UnityEngine;

public class WaitForSecondsSkippable : CustomYieldInstruction
{
    private float waitTime;
    private System.Func<bool> condition;

    public WaitForSecondsSkippable(float time, System.Func<bool> condition)
    {
        waitTime = time + Time.time;
        this.condition = condition;
    }

    public override bool keepWaiting
    {
        get { return Time.time < waitTime && !condition(); }
    }
}