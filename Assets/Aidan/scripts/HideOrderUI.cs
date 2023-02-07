using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrderUI : MonoBehaviour
{
    bool hiddenOnce;

    private void OnEnable()
    {
        if (!hiddenOnce) return;
        AudioManager.instance.PlaySound(7, gameObject);
    }

    public void PlaySound()
    {
        hiddenOnce = true;
        AudioManager.instance.PlaySound(7);
    }

}
