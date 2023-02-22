using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrderUI : MonoBehaviour
{
    bool hiddenOnce;

    private void OnEnable()
    {
        if (hiddenOnce) return;
        AudioManager.instance.PlaySound(23, gameObject);
    }

    public void PlaySound()
    {
        hiddenOnce = true;
        AudioManager.instance.PlaySound(24);
    }

}
