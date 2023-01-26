using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolipCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI toolTipText;

    private void Start() {
        KitchenManager.instance.ttCoord = this;
        toolTipText.text = "";
    }

    public void DisplayItem(Item toDisplay) {
        toolTipText.text = toDisplay.description;
    }
    public void ClearText(Item toClear) {
        if (toolTipText.text == toClear.description) toolTipText.text = "";
    }
}
