using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkspaceCoordinator : MonoBehaviour
{
    [SerializeField] List<Item> heldItems = new List<Item>();
    [SerializeField] List<SpriteRenderer> itemDisplays = new List<SpriteRenderer>();
    [SerializeField] bool forceUpdateDisplays;

    private void OnValidate()
    {
        UpdateItemDisplay();

        if (!forceUpdateDisplays && transform.childCount == itemDisplays.Count) return;
        forceUpdateDisplays = false;

        itemDisplays.Clear();
        for (int i = 0; i < transform.childCount; i++) FormatGOAsItemDisplay(transform.GetChild(i).gameObject);
    }

    void AddNewChildObject()
    {
        var newChild = new GameObject();
        if (transform.childCount > 0) newChild.transform.localScale = transform.GetChild(0).transform.localScale;

        FormatGOAsItemDisplay(newChild);
    }

    void FormatGOAsItemDisplay(GameObject GO)
    {
        var Srend = GO.GetOrAddComponent<SpriteRenderer>();
        itemDisplays.Add(Srend);

        return;
        int index = itemDisplays.Count;
        var setNum = GetSetFromMember(itemDisplays.Count);
        GO.name = "Item Display: " + setNum + " - "+ (index - GetSetStart(setNum));
    }

    public void AddItem(Item newItem)
    {
        heldItems.Add(newItem);
        UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        HideAllItems();
        DisplayItems(heldItems.Count);
    }

    void HideAllItems()
    {
        foreach (var i in itemDisplays) i.enabled = false;
    }

    void DisplayItems(int count)
    {
        int start = GetSetStart(count);
        int end = count == 1 ? 1 : start + count;

        for (int i = start; i < end; i++) {
            print("i: " + i + ", start: " + start + ", end: " + end);
            if (itemDisplays.Count == count-1) AddNewChildObject();
            itemDisplays[i].sprite = heldItems[i-start].GetItemSprite();
            itemDisplays[i].enabled = true;
        }
    }

    int GetSetStart(int count)
    {
        count -= 1;
        float num = (count * count) + count;
        return (int)num / 2;
    }

    int GetSetFromMember(int memberIndex)
    {
        int setNum = 1;

        while (setNum < 100) {
            var setStart = GetSetStart(setNum);
            if (setStart > memberIndex) break;
        }
        return setNum - 1;
    }
}
