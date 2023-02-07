using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Drawing;

public class WorkspaceCoordinator : MonoBehaviour
{
    [SerializeField] List<Transform> itemSlots = new List<Transform>();
    [SerializeField] Transform bigSlot;
    [SerializeField] CookPromptCoordinator cookPrompt;
    ItemCoordinator bigItem;
    [SerializeField] bool displayWhenNotSelected;

    public List<ItemCoordinator> iCoords { get; private set; } = new List<ItemCoordinator>();
    public int capacity { get { return itemSlots.Count; }}
    public bool IsFull { get { return capacity > iCoords.Count; } }
    public int HeldItemCount { get { return iCoords.Count + (bigItem ? 1 : 0); } }

    private void OnValidate()
    {
        UpdateItemDisplay();
    }

    private void Start()
    {
        HideSprites();

    }

    void HideSprites()
    {
        if (bigSlot) {
            bigSlot.SetAsFirstSibling();
            bigSlot.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (cookPrompt) {
            cookPrompt.transform.SetAsFirstSibling();
            cookPrompt.Hide();
        }

        foreach (Transform t in itemSlots) t.GetComponent<SpriteRenderer>().enabled = false;
    }

    void ShowSprites()
    {
        if (bigSlot) {
            bigSlot.SetAsFirstSibling();
            bigSlot.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (cookPrompt) cookPrompt.transform.SetAsFirstSibling();
        foreach (Transform t in itemSlots) t.GetComponent<SpriteRenderer>().enabled = true;
    }

    bool roomForBigEquipment()
    {
        return bigItem == null && bigSlot != null;
    }

    public void SetPromptvalue(float value) {
        if (!cookPrompt) return;
        cookPrompt.SetSlider(value);
    }
    public void DisplayPrompt() {
        if (!cookPrompt) return;
        StartProgressBar();
    }
    public void DisplayPrompt(Item possibleResult) {
        if (!cookPrompt) return;

        StartProgressBar(possibleResult);
    }
    private void StartProgressBar(Item result = null) {
        cookPrompt.gameObject.SetActive(true);
        if (result) cookPrompt.Begin();
        else cookPrompt.Begin();
    }
   
    public void HideCookPrompt() {
        if (!cookPrompt) return;
        cookPrompt.Hide();
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
        int normalItemCapacity = capacity;
        int bigEquipmentCapacity = roomForBigEquipment() ? 1 : 0;

        foreach (var toAdd in listToAdd) {
            if (toAdd.isBigEquipment) bigEquipmentCapacity -= 1;
            else normalItemCapacity -= 1;
        }
        foreach (var i in iCoords) {
            if (i.GetItem().isBigEquipment) bigEquipmentCapacity -= 1;
            else normalItemCapacity -= 1;
        }
        return normalItemCapacity >= 0 && bigEquipmentCapacity >= 0;
    }

    public void AddItem(ItemCoordinator iCoord)
    {

        iCoord.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (iCoord.GetItem().isBigEquipment) bigItem = iCoord;
        else iCoords.Add(iCoord);
        UpdateItemDisplay();
    }

    public void removeItem(ItemCoordinator toRemove)
    {
        iCoords.Remove(toRemove);
        if (toRemove == bigItem) bigItem = null;
        UpdateItemDisplay();
        GetComponent<WorkspaceController>().HaltRecipe();
    }

    public ItemCoordinator removeItem(Item toRemove) {
        var coord = default(ItemCoordinator);
        for (int i = 0; i < iCoords.Count; i++) {
            if (iCoords[i].GetItem().Equals(toRemove)) {
                coord = iCoords[i];
                break;
            }
        }
        if (toRemove.Equals(bigItem.GetItem()) ) {
            print("big!");
            coord = bigItem;
            bigItem = null;
        }
        iCoords.Remove(coord);
        UpdateItemDisplay();
        return coord;
    }

    void UpdateItemDisplay()
    {
        UpdateBigItemDisplay();
        for (int i = 0; i < iCoords.Count; i++) {
            iCoords[i].SetDisplayParent(itemSlots[i], this);
        }
    }

    void UpdateBigItemDisplay()
    {
        if (!bigSlot || !bigItem) return;
        bigItem.SetDisplayParent(bigSlot, this);
        bigItem.gameObject.SetActive(true);
    }

    public List<Item> GetHeldItems() {
        var items = new List<Item>();
        foreach(var i in iCoords) items.Add(i.GetItem());
        if (bigItem) items.Add(bigItem.GetItem());
        return items;
    }

    private void OnDrawGizmosSelected()
    {
        print("hiiii");
        StopAllCoroutines();
        ShowSprites();
        StartCoroutine(HideSpritesAfterDelay());
    }

    IEnumerator HideSpritesAfterDelay()
    {
        print("startingEnumerator");
        yield return new WaitForSeconds(0.5f);
        HideSprites();
    }
}
