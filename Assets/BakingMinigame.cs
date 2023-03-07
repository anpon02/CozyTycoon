using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BakingMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] float progressSpeed, progressPenalty = 0.1f;
    [SerializeField] int flameSound, sheetpullSound;
    [SerializeField] AudioSource oneShotsSource, loopSource;
    bool open;
    private void OnEnable()
    {
        AudioManager.instance.PlaySound(flameSound, loopSource);
        uiCoord.ongoingMinigames += 1;
        open = false;
        Click();
    }

    public void Click()
    {
        AudioManager.instance.PlaySound(sheetpullSound,oneShotsSource);
        open = !open;
        buttonText.text = open ? "Close" : "Check";
        uiCoord.showProgress = open;

        if (open && uiCoord.progressSlider.value >= 1) Complete();
        else if (open) uiCoord.AddProgress(-progressPenalty);
    }

    private void FixedUpdate()
    {
        if (!open) {
            uiCoord.AddProgress(progressSpeed);
        }
    }

    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.showProgress = true;
        uiCoord.CompleteRecipe();
    }
}
