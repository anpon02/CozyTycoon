using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceCoordinator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] SpriteRenderer item1;
    [SerializeField] List<SpriteRenderer> item2 = new List<SpriteRenderer>();

    [Space()]
    [SerializeField] List<Item> heldItems = new List<Item>();

    private void OnValidate()
    {
        UpdateItemDisplay();
    }

    public void AddItem(Item newItem)
    {
        heldItems.Add(newItem);
        UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        HideAllItems();
        if (heldItems.Count == 1) DisplayOneItem();
        if (heldItems.Count == 2) DisplayTwoItems();
    }

    void HideAllItems()
    {
        item1.enabled = false;
        foreach (var i in item2) i.enabled = false;
    }

    void DisplayOneItem()
    {
        if (heldItems[0] == null) return;
        item1.sprite = heldItems[0].GetItemSprite();
        item1.enabled = true;
    }

    void DisplayTwoItems()
    {
        if (heldItems[0] == null || heldItems[1] == null) return;
        for (int i = 0; i < 2; i++) item2[i].sprite = heldItems[i].GetItemSprite();
        item2[0].enabled = item2[1].enabled = true;
    }


}
