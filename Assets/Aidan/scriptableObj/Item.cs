using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum FoodType { NONE, VEGGIE, MEAT };

[CreateAssetMenu(fileName = "newItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject {
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;
    [SerializeField] int throwSoundID;
    public int value = 10;
    [SerializeField] float _scaleMult = 1;
    public bool isBigEquipment;
    public bool menuItem;
    public string description;

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
