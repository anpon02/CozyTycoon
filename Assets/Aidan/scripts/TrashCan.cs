using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    ChefController chef;

    public void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = hoverColor;
    }

    public void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown()
    {
        if (!SetChef() || !chef.IsHoldingItem()) return;

         KitchenManager.instance.lastTrashedItem = chef.RemoveHeldItem();
    }

    bool SetChef()
    {
        chef = KitchenManager.instance.chef;
        return chef != null;
    }
}
