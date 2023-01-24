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
    [SerializeField] float quality = 1;
    [SerializeField] bool disableQuality;
    [SerializeField] int throwSoundID;

    public Sprite GetSprite() {
        return itemSprite;
    }
    public string GetName() {
        return itemName;
    }

    public int GetThrowSound()
    {
        return throwSoundID;
    }

    public void SetQuality(float value)
    {
        quality = value;
    }

    public float GetQuality()
    {
        if (disableQuality) return -1;
        return quality;
    }

    public bool IsBigEquipment()
    {
        return bigEquipment;
    }

    public bool Equals(Item other)
    {
        if (other == null) return false;
        return string.Equals(other.GetName(), GetName());
    }

    public bool IsPresentInList(List<Item> list)
    {
        if (list.Count == 0) return false;

        bool found = false;
        foreach (var i in list) {
            if (Equals(i)) found = true;
        }
        return found;
    }

    public void RemoveFromList(List<Item> list)
    {
        int index = 0;
        for (int i = 0; i < list.Count + 1; i++) {
            if (i > list.Count) return;
            if (Equals(list[i])) break;
        }
        list.RemoveAt(index);
    }
}
