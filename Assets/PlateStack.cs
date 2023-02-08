using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStack : MonoBehaviour
{
    [SerializeField] Color hoverColor;

    private void OnMouseEnter() {
        GetComponent<SpriteRenderer>().color = hoverColor;
    }
    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown() {
        ItemCoordinator iCoord = KitchenManager.instance.chef.GetHeldiCoord();
        if (iCoord) iCoord.plated = true;
    }
}
