using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;

    public Sprite GetSprite() {
        return itemSprite;
    }
    public string GetName() {
        return itemName;
    }
}
