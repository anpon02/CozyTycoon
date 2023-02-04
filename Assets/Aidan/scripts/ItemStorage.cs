using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [Header("Misc")]
    public Item storedItem;
    [SerializeField] UpgradePromptCoordinator upgradeCoord;
    [SerializeField] string toolTip;
    [SerializeField, Range(0, 1)] float itemStartQuality = 0;
    List<ItemCoordinator> dispensedItems = new List<ItemCoordinator>();
    [SerializeField] bool limitedStock;
    [SerializeField, ConditionalHide(nameof(limitedStock))] int numItems;
    int itemsRemaining;

    [Header("Upgrades")]
    public int upgradeCost;
    public string upgradeName = "knife block";
    [SerializeField] float upgradeQualityIncrease;
    [SerializeField, ConditionalHide(nameof(limitedStock))] int upgradeNumIncrease;
    [TextArea(3, 5)] public string upgradeDetails = "";
    bool closed;
    bool upgradedToday;

    [Header("Alt Items")]
    public List<Item> altItems = new List<Item>();
    [SerializeField] ItemSelectorCoordinator itemSelectorCoord;
    bool switchedItem;

    private void Start()
    {
        GameManager.instance.OnStoreClose.AddListener(OnStoreClose);
        GameManager.instance.OnStoreOpen.AddListener(OnStoreOpen);
    }

    void OnStoreClose()
    {
        closed = true;
        for (int i = 0; i < dispensedItems.Count; i++) {
            if (dispensedItems[i] != null) Destroy(dispensedItems[i].gameObject);
        }
        dispensedItems.Clear();
    }

    void OnStoreOpen() {
        closed = false; 
        upgradedToday = false;
        itemsRemaining = numItems;
    }

    private void Update()
    {
        itemSelectorCoord.gameObject.SetActive(altItems.Count > 0 && !closed && InReach());
        if (closed) CheckUpgrade();
        else upgradeCoord.gameObject.SetActive(false);
    }

    void CheckUpgrade()
    {
        if (!upgradedToday) upgradeCoord.gameObject.SetActive(InReach());
    }

    bool InReach()
    {
        var chef = KitchenManager.instance.chef;
        if (!chef) return false;
        return Vector2.Distance(chef.transform.position, transform.position) <= KitchenManager.instance.playerReach;
    }

    public void Upgrade()
    {
        if (upgradedToday) {
            upgradeCoord.gameObject.SetActive(false);
            return;
        }
        upgradedToday = true;
        var w = GameManager.instance.wallet;
        if (w.money >= upgradeCost) w.money -= upgradeCost;
        else return;

        upgradeCoord.gameObject.SetActive(false);
        itemStartQuality = Mathf.Min(1, itemStartQuality + upgradeQualityIncrease);
        if (limitedStock) numItems += upgradeNumIncrease;
    }

    private void OnMouseDown()
    {
        StartCoroutine(waitForMouseUp());
    }

    void GetItemFromStorage()
    {
        if (!KitchenManager.instance || closed) return;

        if (limitedStock && itemsRemaining <= 0) return;
        else if (limitedStock) itemsRemaining -= 1;

        var chef = KitchenManager.instance.chef;
        if (chef == null) return;
        if (chef.GetHeldItem() != null) return;

        var coord = KitchenManager.instance.CreateNewItemCoord(storedItem, transform.position, itemStartQuality);
        dispensedItems.Add(coord);
        chef.HoldNewItem(coord);
    }

    IEnumerator waitForMouseUp()
    {
        while (true) {
            if (!Input.GetMouseButton(0)) break;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        if (!switchedItem) GetItemFromStorage();
        switchedItem = false; 
    }

    public void NextItem()
    {
        switchedItem = true;
        if (altItems.Count == 0) return;
        altItems.Add(storedItem);
        storedItem = altItems[0];
        altItems.RemoveAt(0);
    }

    private void OnMouseEnter()
    {
        KitchenManager.instance.ttCoord.Display(toolTip);
    }
    private void OnMouseExit()
    {
        KitchenManager.instance.ttCoord.ClearText(toolTip);
    }
}
