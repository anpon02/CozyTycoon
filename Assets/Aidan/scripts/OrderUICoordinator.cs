using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUICoordinator : MonoBehaviour
{
    [SerializeField] GameObject listElementPrefab;
    [SerializeField] Transform listParent;
    List<OrderListItemCoordinator> listItems = new List<OrderListItemCoordinator>();
    [SerializeField] float orderPatience = 120;
    
    public void AddNew(string itemName)
    {
        var newListItem = Instantiate(listElementPrefab, listParent);
        var listScript = newListItem.GetComponent<OrderListItemCoordinator>();
        listScript.Init(itemName, orderPatience);
        listItems.Add(listScript);
    }

    public void completeItem(string itemName)
    {
        var listItem = FindListItem(itemName);
        if (listItem == null) return;

        listItem.MarkComplete();
    }

    public void RemoveItem(string itemName)
    {
        var listItem = FindListItem(itemName);
        if (listItem == null) return;

        Destroy(listItem.gameObject);
    }

    OrderListItemCoordinator FindListItem(string itemName)
    {
        for (int i = 0; i < listItems.Count; i++) {
            if (string.Equals(listItems[i].GetItemName(),itemName) ) return listItems[i];
        }
        return null;
    }
}
