using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class WorkspaceCoordinator : MonoBehaviour
{
    [System.Serializable]
    public class DisplayListHolder
    {
        [HideInInspector] public string name;
        [SerializeField] List<SpriteRenderer> displays;

        
        public void ResetAll() {
            foreach (var s in displays) {
                s.sprite = null;
                s.enabled = false;
            }
        }
        public void Set(List<Sprite> sprites) {
            ResetAll();
            for (int i = 0; i < sprites.Count; i++) {
                if (displays.Count == i) break;
                displays[i].sprite = sprites[i];
                displays[i].enabled = true;
            }
        }
        public bool Add(Sprite newSprite) {
            if (FreeSlots() == 0) return false;

            for (int i = 0; i < displays.Count; i++) {
                if (displays[i].enabled) continue;
                else {
                    displays[i].sprite = newSprite;
                    displays[i].enabled = true;
                    return true;
                }
            }
            return false;
        }



        public int FreeSlots() {
            int numFree = displays.Count;
            foreach (var d in displays) if (d.enabled) numFree -= 1;
            return numFree;
        }
        public int Capacity() {
            return displays.Count;
        }
    }

    [SerializeField] SpriteRenderer bigEquipmentSprite;
    [SerializeField] List<DisplayListHolder> itemDisplays = new List<DisplayListHolder>();
    List<ItemCoordinator> heldItems = new List<ItemCoordinator>();
    ItemCoordinator bigItem;

    private void OnValidate()
    {
        UpdateItemDisplay();

        for (int i = 0; i < itemDisplays.Count; i++) {
            var cap = itemDisplays[i].Capacity();
            string complete = cap == i+1 ? "" : (cap < i+1 ? ": INSUFFICIENT SRENDER" : "TOO MANY SRENDER" );
            itemDisplays[i].name = (i+1) + complete;
        }
    }

    bool roomForBigEquipment()
    {
        return bigEquipmentSprite != null && !bigEquipmentSprite.enabled;
    }

    public int Capacity() {
        int capacity = 0;
        for (int i = 0; i < itemDisplays.Count; i++) {
            int cap = itemDisplays[i].Capacity();
            if (cap > capacity) capacity = cap;
        }
        return capacity;
    }

    public bool HasRoom(Item wantToAdd)
    {
        var newList = new List<Item>();
        newList.Add(wantToAdd);
        return HasRoom(newList);
    }

    public bool HasRoom(List<Item> listToAdd)
    {
        int normalItemCapacity = Capacity();
        int bigEquipmentCapacity = roomForBigEquipment() ? 1 : 0;

        foreach (var toAdd in listToAdd) {
            if (toAdd.IsBigEquipment()) bigEquipmentCapacity -= 1;
            else normalItemCapacity -= 1;
        }
        return normalItemCapacity >= 0 && bigEquipmentCapacity >= 0;
    }

    public bool IsFull() {
        return Capacity() > heldItems.Count;
    }

    public int HeldItemCount() {
        return heldItems.Count + (bigItem ? 1 : 0);
    }

    public void AddItem(ItemCoordinator newItem)
    {
        if (newItem.GetItem().IsBigEquipment()) bigItem = newItem;
        else heldItems.Add(newItem);
        UpdateItemDisplay();
    }
    public ItemCoordinator removeItem(Item toRemove) {
        var coord = default(ItemCoordinator);
        for (int i = 0; i < heldItems.Count; i++) {
            if (heldItems[i].GetItem().Equal(toRemove)) {
                coord = heldItems[i];
                break;
            }
        }
        heldItems.Remove(coord);
        return coord;
    }

    public ItemCoordinator removeItem() {
        ItemCoordinator item = null;
        if (heldItems.Count == 0) {
            item = bigItem;
            bigItem = null;
        }
        else {
            item = heldItems[heldItems.Count - 1];
            heldItems.Remove(item);
        }
        UpdateItemDisplay();
        return item;
    }

    public void UpdateItemDisplay()
    {
        HideAllItems();
        DisplayItems(heldItems.Count);
    }

    void HideAllItems()
    {
        foreach (var i in itemDisplays) i.ResetAll();
    }

    List<Sprite> GetItemSpriteLists(List<ItemCoordinator> itemList) 
    {
        var spriteList = new List<Sprite>();
        foreach (var i in itemList) spriteList.Add(i.GetItemSprite());
        return spriteList;
    }

    void DisplayItems(int count)
    {
        if (bigEquipmentSprite) DisplayBigItem();
        for (int i = 0; i < itemDisplays.Count; i++) {
            if (i + 1 == count) itemDisplays[i].Set(GetItemSpriteLists(heldItems));
        }
    }

    void DisplayBigItem()
    {
        bigEquipmentSprite.enabled = bigItem != null;
        bigEquipmentSprite.sprite = bigItem ? bigItem.GetItemSprite() : null;
    }

    public List<Item> GetHeldItems() {
        var items = new List<Item>();
        foreach(var i in heldItems) items.Add(i.GetItem());
        if (bigItem) items.Add(bigItem.GetItem());
        return items;
    }
}
