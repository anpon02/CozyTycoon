using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class startCookingButton : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] WorkstationUICoordinator uiCoord;

    public void StartCooking() {

        if (Input.GetMouseButton(1)) return;

        uiCoord.buttonHovered = false;
        uiCoord.SelectRecipeOption(ID);
    }
}
