
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

    [Header("completion")]
    [SerializeField] Gradient progressGradient;

    [Header("Dependencies")]
    [SerializeField] Slider progressSlider;
    [SerializeField] Image sweetSpot;
    [SerializeField] GameObject endHandle;
    [SerializeField] Image fill;

    public void Hide()
    {
        progressSlider.value = 0;
        gameObject.SetActive(false);
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

    public void SetSlider(float value) {

        fill.color = progressGradient.Evaluate(value);
        gameObject.SetActive(true);
        progressSlider.value = value; 
    }
}
