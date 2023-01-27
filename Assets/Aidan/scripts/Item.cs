using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "newItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject {
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;
    [SerializeField] bool disableQuality;
    [SerializeField] int throwSoundID;
    [SerializeField] string topQualDescript;
    [SerializeField] string medQualDescript;
    [SerializeField] string lowQualDescript;
    public bool isBigEquipment;
    [SerializeField] float _scaleMult = 1;
    public float scaleMult { get {return _scaleMult;} }

    [SerializeField] private float _quality;
    public float quality { get { if (disableQuality) return -1; return _quality;  } set { _quality = value; } }
    public string description { get { return GetDescription(); } }

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

    string GetDescription() {
        Vector2 cutoffs = KitchenManager.instance.midHighQualityCutoff;
        if (quality > cutoffs.y) return topQualDescript;
        if (quality > cutoffs.x) return medQualDescript;
        return lowQualDescript;
    }
}
