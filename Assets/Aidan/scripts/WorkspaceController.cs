using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum WorkspaceType {COUNTER, OVEN, STOVE, SINK}

[RequireComponent(typeof(WorkspaceCoordinator)), RequireComponent(typeof(AudioSource))]
public class WorkspaceController : MonoBehaviour
{
    [SerializeField] WorkspaceType _workspaceType;
    public WorkspaceType workSpaceType { get; private set; }


    public Vector3 itemLerpTarget;
    [SerializeField] int actionSoundID;
    public WorkstationUICoordinator wsUIcoord;
    WorkspaceCoordinator wsCoord;

    [SerializeField] List<ItemCoordinator> iCoords;

    [HideInInspector] public string chosenRecipe;

    KitchenManager kMan;
    AudioSource source;
    [HideInInspector] public bool hasBigEquipment, minigameActive;

    Item result;
    List<Item> toRemove = new List<Item>();
    Minigame minigame;

    private void OnValidate()
    {
        workSpaceType = _workspaceType;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && iCoords.Count > 0 && kMan.hoveredController == this) {

            if (!kMan.chef.IsHoldingItem() || kMan.chef.GetHeldiCoord().CanAccept(GetRighClickItem().GetItem())) 
                RighClickOnWS();
        }

        if (kMan.hoveredController == this) wsUIcoord.ShowRecipeOptions(GetValidRecipeResults());
        else wsUIcoord.HideRecipeOptions();
    }

    void RighClickOnWS()
    {
        if (wsUIcoord.IsMinigameActive()) return;

        var iCoord = GetRighClickItem();
        if (iCoord.GetItem().isBigEquipment) hasBigEquipment = false;
        iCoords.Remove(iCoord);
        if (iCoord == null) return;
        iCoord.gameObject.SetActive(true);
        kMan.lastRetrievedItem = iCoord.GetItem();

        if (!kMan.chef.IsHoldingItem()) { kMan.chef.PickupItem(iCoord); return; }
        kMan.chef.GetHeldiCoord().AddItem(iCoord.GetItem());
        kMan.chef.JustGotItem = true;
        Destroy(iCoord.gameObject);
    }

    ItemCoordinator GetRighClickItem()
    {
        var iCoord = iCoords[iCoords.Count - 1];
        foreach (var i in iCoords) {
            if (iCoord.GetItem().isBigEquipment && !i.GetItem().isBigEquipment) { iCoord = i; break; }
        }
        return iCoord;
    }

    private void Start()
    {
        wsCoord = GetComponent<WorkspaceCoordinator>();
        OnValidate();
        itemLerpTarget += transform.position;
        kMan = KitchenManager.instance;
        GameManager.instance.OnStoreClose.AddListener(RefreshList);
    }

    void RefreshList() {
        StartCoroutine(WaitThenRefresh());
    }

    IEnumerator WaitThenRefresh() {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < iCoords.Count; i++) {
            if (iCoords[i] == null) iCoords.RemoveAt(i);
        }
        hasBigEquipment = false;
        foreach (var i in iCoords) if (i.GetItem().isBigEquipment) hasBigEquipment = true;
        wsCoord.UpdateItemDisplay();
    }

    public void RemoveItemFromList(int index)
    {
        if (wsUIcoord.IsMinigameActive()) return;

        ItemCoordinator iCoord = null;
        if (iCoords.Count > index) iCoord = iCoords[index];
        if (iCoord == null) return;

        if (kMan.chef.GetHeldiCoord() != null && !kMan.chef.GetHeldiCoord().CanAccept(iCoord.GetItem())) return;

        
        kMan.lastRetrievedItem = iCoord.GetItem();
        if (iCoord.GetItem().isBigEquipment) hasBigEquipment = false;
        iCoords.Remove(iCoord);
        iCoord.gameObject.SetActive(true);

        if (kMan.chef.GetHeldiCoord() == null) { kMan.chef.PickupItem(iCoord); return; }
        kMan.chef.GetHeldiCoord().AddItem(iCoord.GetItem());
        kMan.chef.JustGotItem = true;
        Destroy(iCoord.gameObject);
    }

    public void HaltRecipe()
    {
        if (source != null) source.Stop();
        StopAllCoroutines();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var iCoord = collision.GetComponent<ItemCoordinator>();
        if (iCoord) AddItem(iCoord);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var iCoord = collision.GetComponent<ItemCoordinator>();
        if (iCoord) AddItem(iCoord);
    }
    public List<Item> GetValidRecipeResults()
    {
        var list = new List<Item>();
        var recipes = RecipeManager.instance.GetValidRecipes(GetItemList(), this);
        foreach (var r in recipes) list.Add(r.GetResult());

        return list;
    }

    void AddItem(ItemCoordinator iCoord)
    {
        if (iCoords.Contains(iCoord) || iCoord.travellingToChef || kMan.chef.GetHeldiCoord() == iCoord || (iCoord.wsDest != null && iCoord.wsDest != this)) return;
        bool isBig = iCoord.GetItem().isBigEquipment;
        if (isBig && hasBigEquipment) return;
        if (isBig) hasBigEquipment = true;

        kMan.lastAddedItem = iCoord.GetItem();
        iCoords.Add(iCoord);
        iCoord.gameObject.SetActive(false);
    }

    public List<Item> GetItemList()
    {
        List<Item> list = new List<Item>();
        foreach (var iCoord in iCoords) {
            list.Add(iCoord.GetItem());
        }
        return list;
    }

    public List<ItemCoordinator> GetiCoordList() {
        List<ItemCoordinator> list = new List<ItemCoordinator>();
        foreach (var iCoord in iCoords) {
            list.Add(iCoord);
        }
        return list;
    }

    public void StartCooking()
    {
        if (string.IsNullOrEmpty(chosenRecipe)) return;

        Recipe r = null;
        var rList = RecipeManager.instance.GetValidRecipes(GetItemList(), this);
        foreach (var _r in rList) if (_r.GetResult().GetName() == chosenRecipe) r = _r;
        if (r == null) return;

        result = r.GetResult();
        toRemove = r.GetIngredients();
        minigame = r.GetMinigame();

        GameManager.instance.TEMP_SELECTED_RECIPE = true;
        wsUIcoord.StartMinigame(minigame);
        minigameActive = true;
    }

    public void CompleteRecipe()
    {
        foreach (var item in toRemove) {
            var iCoord = RemoveItem(item);
            if (iCoord) Destroy(iCoord.gameObject);
        }

        ItemCoordinator newResult = kMan.CreateNewItemCoord(result, transform.position);
        AddItem(newResult);
        StartCoroutine(WaitThenMinigameInactive());
    }

    IEnumerator WaitThenMinigameInactive()
    {
        yield return new WaitForSeconds(0.5f);
        minigameActive = false;
    }

    ItemCoordinator RemoveItem(Item toRemove)
    {
        ItemCoordinator iCoord = null;
        for (int i = 0; i < iCoords.Count; i++) {
            if (iCoords[i].GetItem().Equals(toRemove)) iCoord = iCoords[i];
        }
        if (iCoords.Contains(iCoord)) iCoords.Remove(iCoord);
        return iCoord;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + itemLerpTarget, 0.1f);
    }
}
