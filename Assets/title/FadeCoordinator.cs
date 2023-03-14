using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCoordinator : MonoBehaviour
{
    [HideInInspector] public bool animComplete;

    void Start()
    {
        animComplete = false;
    }

    public void AnimDone()
    {
        animComplete = true;
    }
}
