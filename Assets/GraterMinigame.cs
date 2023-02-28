using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraterMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    [SerializeField] GameObject grater, cheese;
    [SerializeField] Vector2 graterLimits;
    [SerializeField] float SpeedMod, winDist = 4;
    bool holding;
    Vector3 oldPos;
    float totalDist;

    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;
        var pos = grater.transform.localPosition;
        pos.y = (graterLimits.y + graterLimits.x) /2;
        grater.transform.localPosition = pos;
    }

    public void Click()
    {
        holding = true;
    }

    private void Update()
    {
        oldPos = grater.transform.position;
        MoveGrater();
        ProgressCheck();
        ScaleCheese();

        if (Input.GetMouseButtonUp(0)) holding = false;
    }

    void ScaleCheese()
    {
        cheese.transform.localScale = Vector3.one * (totalDist / winDist);
    }

    void ProgressCheck()
    {
        var dist = Vector3.Distance(grater.transform.position, oldPos);
        uiCoord.AddProgress(dist / winDist);
        totalDist += dist;
        if (uiCoord.progressSlider.value >= 1) Complete();
    }

    void MoveGrater()
    {
        var mouseY = Input.GetAxis("Mouse Y");
        if (holding) grater.transform.position += Vector3.up * mouseY * SpeedMod;

        var pos = grater.transform.localPosition;
        pos.y = Mathf.Clamp(pos.y, graterLimits.x, graterLimits.y);
        grater.transform.localPosition = pos;
    }

    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
