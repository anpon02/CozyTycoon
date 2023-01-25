using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Drawing;

public class WorkspaceCoordinator : MonoBehaviour
{
    [System.Serializable]
    public class DisplayListHolder
    {
        [HideInInspector] public string name;
        [SerializeField] List<SpriteRenderer> displays;
        List<ItemCoordinator> iCoords = new List<ItemCoordinator>();
        
        public void FakeDisplay(Sprite sprite)
        {
            displays[0].enabled = true;
            displays[0].sprite = sprite;
        }

        public void Add(List<ItemCoordinator> _iCoords, WorkspaceCoordinator _wsCoord)
        {
            foreach (var i in _iCoords) Add(i, _wsCoord);
        }
        public bool Add(ItemCoordinator iCoord, WorkspaceCoordinator _wsCoord) {
            if (FreeSlots() == 0 || iCoords.Contains(iCoord)) return false;

            iCoord.SetDisplayParent(displays[iCoords.Count].transform, _wsCoord);
            iCoord.gameObject.SetActive(true);
            iCoords.Add(iCoord);
            return true;
        }
        public int FreeSlots() {
            return displays.Count - iCoords.Count;
        }
        public int Capacity() {
            return displays.Count;
        }

        public List<ItemCoordinator> removeAll()
        {
            displays[0].enabled = false;
            foreach (var i in iCoords) {
                i.gameObject.SetActive(false);
                i.FreeFromDisplayParent();
            }
            return iCoords;
        }

        public bool Active()
        {
            return iCoords.Count > 0;
        }

    }
    
    [SerializeField] List<DisplayListHolder> itemDisplays = new List<DisplayListHolder>();
    [SerializeField] SpriteRenderer bigEquipmentSprite;
    [SerializeField] CookPromptCoordinator cookPrompt;
    List<ItemCoordinator> iCoords = new List<ItemCoordinator>();
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

    public int GetCurrentQuality()
    {
        if (cookPrompt == null) return -1;
        return cookPrompt.GetSliderDistRating();
    }

    bool roomForBigEquipment()
    {
        return bigEquipmentSprite != null && !bigEquipmentSprite.enabled;
    }

    public bool SetPromptvalue(float value) {
        if (!cookPrompt) return false;
        return cookPrompt.SetSlider(value);
    }
    public void DisplayPrompt() {
        if (!HasCookPrompt()) return;
        StartProgressBar();
    }
    public void DisplayPrompt(Item possibleResult) {
        if (!HasCookPrompt()) return;

        StartProgressBar(possibleResult);
    }
    private void StartProgressBar(Item result = null) {
        cookPrompt.gameObject.SetActive(true);
        if (result) cookPrompt.Begin();
        else cookPrompt.Begin();
    }
    public void previewResult(Sprite itemSprite)
    {
        foreach (var i in itemDisplays) i.removeAll();
        itemDisplays[0].FakeDisplay(itemSprite);
    }
    public void HideCookPrompt() {
        if (!cookPrompt) return;
        cookPrompt.Hide();
    }
    public bool HasCookPrompt() {
        return cookPrompt != null;
    }

    public int Capacity() {
        int capacity = 0;
        for (int i = 0; i < itemDisplays.Count; i++) {
            int cap = itemDisplays[i].Capacity();
            if (cap > capacity) capacity = cap;
        }
        return capacity;
    }

    public bool HasItem (ItemCoordinator iCoord)
    {
        return iCoords.Contains(iCoord);
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
        foreach (var i in iCoords) {
            if (i.GetItem().IsBigEquipment()) bigEquipmentCapacity -= 1;
            else normalItemCapacity -= 1;
        }
        return normalItemCapacity >= 0 && bigEquipmentCapacity >= 0;
    }

    public bool IsFull() {
        return Capacity() > iCoords.Count;
    }

    public int HeldItemCount() {
        return iCoords.Count + (bigItem ? 1 : 0);
    }

    public void AddItem(ItemCoordinator newItem)
    {
        if (true) {
            string s = "items: ";
            foreach (var i in iCoords) s += i.GetItem().GetName() + ", ";
            print(s + ", adding: " + newItem.GetItem().GetName());
        }
        if (newItem.GetItem().IsBigEquipment()) bigItem = newItem;
        else iCoords.Add(newItem);
        UpdateItemDisplay();
    }

    public void removeItem(ItemCoordinator toRemove)
    {
        //can inturrupt cooking process??
        iCoords.Remove(toRemove);
        UpdateItemDisplay();
    }

    public ItemCoordinator removeItem(Item toRemove) {
        var coord = default(ItemCoordinator);
        for (int i = 0; i < iCoords.Count; i++) {
            if (iCoords[i].GetItem().Equals(toRemove)) {
                coord = iCoords[i];
                break;
            }
        }
        iCoords.Remove(coord);
        UpdateItemDisplay();
        return coord;
    }

    void UpdateItemDisplay()
    {
        UpdateBigItemDisplay();
        foreach (var iDisplay in itemDisplays) iDisplay.removeAll();
        for (int i = 0; i < itemDisplays.Count; i++) {
            if (i + 1 == iCoords.Count) itemDisplays[i].Add(iCoords, this);
        }
    }

    void UpdateBigItemDisplay()
    {
        if (!bigEquipmentSprite) return;
        bigEquipmentSprite.enabled = bigItem != null;
        bigEquipmentSprite.sprite = bigItem ? bigItem.GetItemSprite() : null;
    }

    public List<Item> GetHeldItems() {
        var items = new List<Item>();
        foreach(var i in iCoords) items.Add(i.GetItem());
        if (bigItem) items.Add(bigItem.GetItem());
        return items;
    }
}
