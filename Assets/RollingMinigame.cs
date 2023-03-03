using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    [SerializeField] GameObject rollingPin;
    [SerializeField] Image arrow;
    [SerializeField] Vector2 rollPinYLimits;
    [SerializeField] float mouseMod = 0.9f, activexPosition, timesRequired = 6;
    
    Vector3 idlePosition;
    bool holdingDown, goalTop;
    float offset;

    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;
        idlePosition = rollingPin.transform.position;
        ResetPin();
        SetNewGoal();
    }

    void ResetPin()
    {
        holdingDown = false;
        rollingPin.transform.position = idlePosition;
        rollingPin.transform.localEulerAngles = new Vector3(0, 0, -90);
    }

    public void OnClick()
    {
        holdingDown = true;
        rollingPin.transform.localEulerAngles = Vector3.zero;

        var pos = rollingPin.transform.position;
        pos.x = activexPosition;
        rollingPin.transform.position = pos;
        pos = rollingPin.transform.localPosition;
        pos.y = goalTop ? rollPinYLimits.x : rollPinYLimits.y;
        rollingPin.transform.localPosition = pos;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = rollingPin.transform.position.y - mousePos.y;
    }

    void SetNewGoal()
    {
        goalTop = Random.Range(0, 1f) > 0.3f ? !goalTop : (Random.Range(0, 1f) > 0.5f ? true : false);
        arrow.transform.localEulerAngles = new Vector3(0, 0, goalTop ? 0 : 180);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) ResetPin();

        if (holdingDown) MovePin();

        float goal = goalTop ? rollPinYLimits.y : rollPinYLimits.x;
        if (Mathf.Abs(rollingPin.transform.localPosition.y - goal) <= 0.1f) CompleteRoll();
    }

    void CompleteRoll()
    {
        ResetPin();
        SetNewGoal();
        uiCoord.AddProgress(1.0f / timesRequired);
        if (uiCoord.progressSlider.value >= 1) Complete();
    }

    void MovePin()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = rollingPin.transform.position.x;
        mousePos.z = rollingPin.transform.position.z;
        mousePos.y += offset;
        rollingPin.transform.position = Vector3.Lerp(rollingPin.transform.position, mousePos, mouseMod);

        var lPos = rollingPin.transform.localPosition;
        lPos.y = Mathf.Clamp(lPos.y, rollPinYLimits.x, rollPinYLimits.y);
        rollingPin.transform.localPosition = lPos;
    }

    void Complete()
    {
        ResetPin();
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
