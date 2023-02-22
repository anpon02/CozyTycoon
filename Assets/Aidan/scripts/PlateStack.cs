using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStack : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    [SerializeField] SpriteRenderer sRend;
    [SerializeField] int plateSound;

    private void OnMouseEnter() {
        sRend.color = hoverColor;
    }
    private void OnMouseExit() {
        sRend.color = Color.white;
    }

    private void OnMouseDown() {
        ItemCoordinator iCoord = KitchenManager.instance.chef.GetHeldiCoord();
        if (iCoord && iCoord.GetItem().type != FoodType.NONE) { 
            iCoord.plated = true;
            AudioManager.instance.PlaySound(plateSound, gameObject);
        }
         
    }
}
