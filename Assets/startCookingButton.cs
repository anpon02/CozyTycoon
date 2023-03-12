using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class startCookingButton : MonoBehaviour
{
    [SerializeField] Color hoverColor = Color.white;
    Color normalColor = Color.black;
    [SerializeField] int ID;
    [SerializeField] WorkstationUICoordinator uiCoord;

    private void OnEnable() {
        if (normalColor == Color.black || normalColor == Color.white) normalColor = GetComponent<Image>().color;
    }

    private void OnMouseDown() {
        StartCooking();
    }

    public void StartCooking() {
        if (Input.GetMouseButton(1)) return;

        uiCoord.buttonHovered = false;
        uiCoord.SelectRecipeOption(ID);
    }

    private void OnMouseEnter() {
        uiCoord.buttonHovered = true;
        GetComponent<Image>().color = hoverColor;
    }

    private void OnMouseExit() {
        uiCoord.buttonHovered = false;
        GetComponent<Image>().color = normalColor;
    }
}
