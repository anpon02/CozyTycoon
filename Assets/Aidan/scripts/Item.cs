using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "newItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;
    [SerializeField] bool bigEquipment;

    public Sprite GetSprite() {
        return itemSprite;
    }
    public string GetName() {
        return itemName;
    }

    public bool IsBigEquipment()
    {
        return bigEquipment;
    }

    public bool Equal(Item other)
    {
        return string.Equals(other.GetName(), GetName());
    }

    public bool IsPresentInList(List<Item> list)
    {
        if (list.Count == 0) return false;

        bool found = false;
        foreach (var i in list) {
            if (Equal(i)) found = true;
        }
        return found;
    }

    public void RemoveFromList(List<Item> list)
    {

        int index = 0;
        for (int i = 0; i < list.Count + 1; i++) {
            if (i > list.Count) return;
            if (Equal(list[i])) break;
        }
        list.RemoveAt(index);
    }
}
