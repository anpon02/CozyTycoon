using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField] Item storedItem;

    private void OnMouseDown()
    {
        if (!KitchenManager.instance) return;

        var coord = KitchenManager.instance.CreateNewItemCoord(storedItem, transform.position);
        KitchenManager.instance.GetChef().HoldNewItem(coord);
    }
}
