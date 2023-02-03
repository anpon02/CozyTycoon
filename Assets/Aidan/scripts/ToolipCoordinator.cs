using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ToolipCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI toolTipText;

    private void Start() {
        KitchenManager.instance.ttCoord = this;
        toolTipText.text = "";
    }

    public void Display(Item toDisplay) {
        toolTipText.text = toDisplay.description;
        toolTipText.text += toDisplay.quality < KitchenManager.instance.midHighQualityCutoff.x ?
            "     <color=red>(poor)</color>" : (toDisplay.quality > KitchenManager.instance.midHighQualityCutoff.y ?
                "     <color=green>(pristine)</color>" : "     <color=white>(neutral)</color>");
    }
    public void Display(string toDisplay)
    {
        toolTipText.text = toDisplay;
    }
    public void ClearText(Item toClear) {
        if (toolTipText.text.Contains(toClear.description)) toolTipText.text = "";
    }
    public void ClearText(string toClear)
    {
        if (toolTipText.text == toClear) toolTipText.text = "";
    }
}
