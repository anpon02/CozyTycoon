using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        [HideInInspector] public string name;
        public Item item;
        public int maxNum;
        public int numRemaining;
        [HideInInspector] public List<ItemCoordinator> instances = new List<ItemCoordinator>();
        [HideInInspector] public bool enabled;
    }

    [SerializeField] List<ItemData> items;
    [SerializeField] string toolTip;
    [SerializeField] ItemSelectorCoordinator itemSelectorCoord;
    [SerializeField] Color hoveredColor;
    [SerializeField] itemGridCoordinator itemGrid;

    public Item currentItem { get {return items[itemIndex].item; } }
    public int numItems { get { return NumEnabledItems(); } }

    ChefController chef;
    int itemIndex;
    bool closed;
    bool overButton;

    public void HoverButton() {
        overButton = true;
    }

    public void LeaveButton() {
        overButton = false;
    }

    private void OnValidate()
    {
        foreach (var item in items) {
            if (item.item != null) item.name = item.item.GetName();
        }
        itemSelectorCoord.gameObject.SetActive(false);
    }

    public void DisplayGrid()
    {
        var list = new List<ItemData>();
        foreach (var i in items) if (i.enabled) list.Add(i);
        itemGrid.DisplayGrid(list);
    }

    public void Enable(Item toEnable, int quantity = -1)
    {
        foreach (var item in items) {
            if (item.item.Equals(toEnable)) { 
                item.enabled = true;
                if (quantity == -1) item.numRemaining = item.maxNum;
                else item.numRemaining += quantity;
            }
        }
    }

    private void OnEnable()
    {
        if (items[itemIndex].enabled) return;

        int startingIndex = itemIndex;
        while (!items[itemIndex].enabled) {
            itemIndex += 1;
            if (itemIndex >= items.Count) itemIndex = 0;
            if (itemIndex == startingIndex) return;
        }
    }

    public void GetItem()   
    {
        if (closed || !SetChef() || !items[itemIndex].enabled) return;
        if (chef.IsHoldingItem()) { TryRestock(); return; }

        if (items[itemIndex].numRemaining == 0) return;
        items[itemIndex].numRemaining -= 1;

        var coord = KitchenManager.instance.CreateNewItemCoord(items[itemIndex].item, transform.position);
        items[itemIndex].instances.Add(coord);
        coord.home = this;

        chef.PickupItem(coord);
    }

    void TryRestock()
    {
        var heldItem = chef.GetHeldiCoord();
        foreach (var i in items) if (heldItem.GetItem().Equals(i.item)) RestockItem(i, heldItem);
    }

    void RestockItem(ItemData i, ItemCoordinator iCoord)
    {
        chef.RemoveHeldItem();
        if (i.maxNum == 0 && i.enabled) i.numRemaining += 1;
    }

    int NumEnabledItems()
    {
        int num = 0;
        foreach (var i in items) if (i.enabled) num += 1;
        return num;
    }

    bool SetChef()
    {
        chef = KitchenManager.instance.chef;
        return chef != null;
    }

    private void Start()
    {
        KitchenManager.instance.allStorage.Add(this);
        GameManager.instance.OnStoreClose.AddListener(OnStoreClose);
        GameManager.instance.OnStoreOpen.AddListener(OnStoreOpen);
        itemSelectorCoord.gameObject.SetActive(false);
    }    

    void OnStoreClose()
    {
        closed = true;
        DeleteAllInstances();
    }

    void DeleteAllInstances()
    {
        foreach (var item in items) {
            for (int i = 0; i < item.instances.Count; i++) {
                if (item.instances[i] != null) Destroy(item.instances[i].gameObject);
            }
            item.instances.Clear();
        }
    }

    void OnStoreOpen() {
        closed = false;
        foreach (var item in items) item.numRemaining = item.maxNum;
    }

    public void InstanceDestructionCallback(ItemCoordinator destroyed)
    {
        foreach (var item in items) {
            if (item.instances.Contains(destroyed)) {
                item.instances.Remove(destroyed);
                if (item.maxNum > item.numRemaining) item.numRemaining += 1;
            }
        }
    }

    public void SelectItem(int _itemIndex)
    {
        if (_itemIndex == -1) return;

        var list = new List<ItemData>();
        foreach (var i in items) if (i.enabled) list.Add(i);
        var selected = list[_itemIndex];
        
        for (int i = 0; i < items.Count; i++) {
            if (items[i].item.Equals(selected.item)) itemIndex = i;
        }

        if (!KitchenManager.instance.chef.IsHoldingItem()) GetItem();
    }

    private void OnMouseEnter()
    {
        if (NumEnabledItems() == 0) return;
        if (!items[itemIndex].enabled) itemIndex = GetFIrstEnabledItem();

        KitchenManager.instance.ttCoord.Display(toolTip);
        GetComponent<SpriteRenderer>().color = hoveredColor;
        if (NumEnabledItems() > 0) itemSelectorCoord.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (overButton) return;

        KitchenManager.instance.ttCoord.ClearText(toolTip);
        GetComponent<SpriteRenderer>().color = Color.white;
        itemSelectorCoord.gameObject.SetActive(false);
    }

    int GetFIrstEnabledItem() {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].enabled) return i;
        }
        return 0;
    }
}
