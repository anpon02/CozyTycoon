using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    [SerializeField] WorkspaceCoordinator wsCoord;
    [SerializeField] GameObject pot, fire;
    [SerializeField] Vector2 potYlimits, potSpeedLimits = new Vector2(-10, 10), fireScaleLimits = new Vector2(0.1f, 2), changeAmountRange = new Vector2(1, 3), goodZone;
    [SerializeField] float potSpeed, progressSpeed;
    [SerializeField] int potcookSound;

    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;

        uiCoord.bigEquipmentSprite.SetActive(false);
        wsCoord.hideItems = true;
        AudioManager.instance.PlaySound(potcookSound, gameObject);
    }

    public void IncreaseSpeed()
    {
        potSpeed += Random.Range(changeAmountRange.x, changeAmountRange.y);
    }

    public void DecreaseSpeed()
    {
        potSpeed -= Random.Range(changeAmountRange.x, changeAmountRange.y);
    }

    private void FixedUpdate()
    {
        MovePot();
        SetFireScale();
        if (PotInGoodZone()) uiCoord.AddProgress(progressSpeed);
        if (uiCoord.progressSlider.value >= 1) Complete();
    }

    bool PotInGoodZone()
    {
        return pot.transform.localPosition.y > goodZone.x && pot.transform.localPosition.y < goodZone.y;
    }

    void MovePot()
    {
        potSpeed = Mathf.Clamp(potSpeed, potSpeedLimits.x, potSpeedLimits.y);
        var pos = pot.transform.localPosition + (Vector3.up * potSpeed * Time.deltaTime);
        pos.y = Mathf.Clamp(pos.y, potYlimits.x, potYlimits.y);
        pot.transform.localPosition = pos;
    }

    void SetFireScale()
    {
        var scale = fire.transform.localScale;
        float potSpeedNormalized = (potSpeed + Mathf.Abs(potSpeedLimits.x)) / Mathf.Abs(potSpeedLimits.x - potSpeedLimits.y);
        scale.y = potSpeedNormalized * Mathf.Abs(fireScaleLimits.x - fireScaleLimits.y)  + fireScaleLimits.x;
        fire.transform.localScale = scale;
    }

    void Complete()
    {
        uiCoord.bigEquipmentSprite.SetActive(true);
        wsCoord.hideItems = false;
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
