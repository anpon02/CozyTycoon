
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CookPromptCoordinator : MonoBehaviour
{
    [Header("Text Label")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color cookPromptFoodColor;
    [SerializeField] GameObject textParent;

    [Header("Dependencies")]
    [SerializeField] Slider progressSlider;
    [SerializeField] Image sweetSpot;
    [SerializeField] GameObject endHandle;
    [SerializeField] Image fill;

    [Header("Sweet spot")]
    [SerializeField] float sweetSpotSize = 0.5f;
    [Range(0,1)]
    [SerializeField] float sweetSpotStart = 0;
    [SerializeField] Vector2 leftRightPos;
    [SerializeField] Vector2 distanceCutoff = new Vector2(0.5f, 1.5f);

    [Header("completion")]
    [SerializeField] Gradient progressGradient;
    [SerializeField, Range(0,1)] float completionThreshold;
    [SerializeField] RectTransform completionBackground;

    [SerializeField] bool debugToggle;
    [SerializeField, ConditionalHide(nameof(debugToggle))]
    GameObject leftGold, rightGold, leftSilver, rightSilver, leftBronze, rightBronze;

    void ShowDebug()
    {
        SetActiveDebug(debugToggle);
        if (!debugToggle) return;

        leftGold.transform.position = sweetSpot.transform.position + Vector3.left * distanceCutoff.x;
        rightGold.transform.position = sweetSpot.transform.position + Vector3.right * distanceCutoff.x;

        leftSilver.transform.position = sweetSpot.transform.position + Vector3.left * distanceCutoff.y;
        rightSilver.transform.position = sweetSpot.transform.position + Vector3.right * distanceCutoff.y;

        leftBronze.SetActive(false);
        rightBronze.SetActive(false);
        //leftBronze.transform.position = sweetSpot.transform.position + Vector3.left * (distanceCutoff.y + 0.1f);
        //rightBronze.transform.position = sweetSpot.transform.position + Vector3.right * (distanceCutoff.y + 0.1f);
    }

    void SetActiveDebug(bool active)
    {
        leftGold.SetActive(active);
        rightGold.SetActive(active);
        leftSilver.SetActive(active);
        rightSilver.SetActive(active);
        leftBronze.SetActive(active);
        rightBronze.SetActive(active);
    }

    private void OnValidate()
    {
        SetSweetSpotVisual();

        var current = completionBackground.sizeDelta;
        current.x = completionThreshold * 120;
        completionBackground.sizeDelta = current;

        GetSliderDistRating();
        ShowDebug();
    }

    void SetSweetSpotVisual()
    {
        if (sweetSpot == null) return;

        var rect = sweetSpot.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(sweetSpotSize, rect.sizeDelta.y);
        var pos = sweetSpot.transform.localPosition;
        pos.x = RangeToPos(sweetSpotStart);
        sweetSpot.transform.localPosition = pos;
    }

    public void Hide()
    {
        progressSlider.value = 0;
        gameObject.SetActive(false);
    }

    float RangeToPos(float start) 
    {
        var total = Mathf.Abs(leftRightPos.y - leftRightPos.x);
        var offset = total * start;
        return Mathf.Min(leftRightPos.x, leftRightPos.y) + offset;
    }

    private void Start() {
        gameObject.SetActive(!Application.isPlaying);
    }

    public void Begin() {        
        textParent.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Begin(Item result) {
        gameObject.SetActive(true);
        textParent.SetActive(true);
        SetText(result);
    }

    void SetText(Item item) {
        text.text = "Making <color=#" + ColorUtility.ToHtmlStringRGB(cookPromptFoodColor) + ">" + item.GetName() + "</color>...";
    }

    public bool SetSlider(float value) {
        if (value < completionThreshold) fill.color = progressGradient.Evaluate(value / completionThreshold);
        else fill.color = progressGradient.Evaluate(1);

        gameObject.SetActive(true);
        progressSlider.value = value;
        return value >= completionThreshold; 
    }

    public int GetSliderDistRating()
    {
        var dist = Mathf.Abs(endHandle.transform.position.x - sweetSpot.transform.position.x);
        int value;

        if (dist < distanceCutoff.x) value = 0;
        else if (dist < distanceCutoff.y) value = 1;
        else value = 2;
        //print("dist: " + dist + ", rating: " + value);

        return value;
    }

}
