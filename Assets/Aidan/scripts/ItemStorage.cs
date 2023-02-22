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
        //[HideInInspector] public int numRemaining;
        public int numRemaining;
        [HideInInspector] public List<ItemCoordinator> instances = new List<ItemCoordinator>();
        [HideInInspector] public bool enabled;
    }

    [SerializeField] List<ItemData> items;
    [SerializeField] string toolTip;
    [SerializeField] ItemSelectorCoordinator itemSelectorCoord;
    [SerializeField] Color hoveredColor;

    public Item currentItem { get {return items[itemIndex].item; } }
    public int numItems { get { return NumEnabledItems(); } }

    ChefController chef;
    int itemIndex;
    bool closed;

    private void OnValidate()
    {
        foreach (var item in items) {
            if (item.item != null) item.name = item.item.GetName();
        }
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
        gameObject.SetActive(NumEnabledItems() > 0);
    }

    public void GetItem()
    {
        if (closed || !SetChef() || chef.IsHoldingItem() || !items[itemIndex].enabled) return;

        if (items[itemIndex].numRemaining == 0) return;
        items[itemIndex].numRemaining -= 1;

        var coord = KitchenManager.instance.CreateNewItemCoord(items[itemIndex].item, transform.position);
        items[itemIndex].instances.Add(coord);
        coord.home = this;

        chef.PickupItem(coord);
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
                item.numRemaining += 1;
            }
        }
    }

    public void NextItem()
    { 
        itemIndex += 1;
        if (itemIndex >= items.Count) itemIndex = 0;
        if (NumEnabledItems() > 0 && !items[itemIndex].enabled) NextItem();
    }

    private void OnMouseEnter()
    {
        if (NumEnabledItems() == 0) return;

        KitchenManager.instance.ttCoord.Display(toolTip);
        GetComponent<SpriteRenderer>().color = hoveredColor;
        itemSelectorCoord.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        KitchenManager.instance.ttCoord.ClearText(toolTip);
        GetComponent<SpriteRenderer>().color = Color.white;
        itemSelectorCoord.gameObject.SetActive(false);
    }
}
