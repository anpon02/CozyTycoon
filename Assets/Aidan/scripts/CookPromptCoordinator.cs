using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CookPromptCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color cookPromptFoodColor;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject textParent;
    [SerializeField] float progressSteps = 100;

    private void Start() {
        gameObject.SetActive(false);
    }

    public void Begin() {        
        textParent.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Begin(Item result) {
        gameObject.SetActive(true);
        textParent.SetActive(false);
        SetText(result);
    }

    void SetText(Item item) {
        text.text = "Making <color=#" + ColorUtility.ToHtmlStringRGB(cookPromptFoodColor) + ">" + item.GetName() + "</color>...";
    }

    public void SetSlider(float value) {
        gameObject.SetActive(true);
        progressSlider.value = value;
    }

}
