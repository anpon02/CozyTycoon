using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ToolipCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI toolTipText;
    [SerializeField] Image textBacking;
    [SerializeField] string fontTag;

    private void Start() {
        KitchenManager.instance.ttCoord = this;
        toolTipText.text = "";
    }

    private void Update()
    {
        textBacking.rectTransform.sizeDelta = new Vector2(toolTipText.preferredWidth, textBacking.rectTransform.sizeDelta.y);
    }

    public void Display(Item toDisplay) {
        toolTipText.text = fontTag + toDisplay.description;
    }
    public void Display(string toDisplay)
    {
        toolTipText.text = fontTag + toDisplay;
    }
    public void ClearText(Item toClear) {
        if (toolTipText.text.Contains(toClear.description)) toolTipText.text = "";
    }
    public void ClearText(string toClear)
    {
        if (toolTipText.text == toClear) toolTipText.text = "";
    }
}
