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
    [SerializeField] WorkstationUICoordinator wsUIcoord;

    [SerializeField] List<ItemCoordinator> iCoords;

    public string chosenRecipe;

    KitchenManager kManag;
    AudioSource source;

    Item result;
    List<Item> toRemove = new List<Item>();
    Minigame minigame;

    private void OnValidate()
    {
        workSpaceType = _workspaceType;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && iCoords.Count > 0 && !kManag.chef.IsHoldingItem() && KitchenManager.instance.hoveredController == this) {
            var iCoord = iCoords[iCoords.Count - 1];
            iCoords.Remove(iCoord);
            iCoord.gameObject.SetActive(true);
            kManag.chef.PickupItem(iCoord);
        }
    }

    private void Start()
    {
        itemLerpTarget += transform.position;
        kManag = KitchenManager.instance;
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
        if (iCoords.Contains(iCoord) || iCoord.travellingToChef) return;
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

        wsUIcoord.StartMinigame(minigame);
    }

    public void CompleteRecipe()
    {
        foreach (var item in toRemove) {
            var iCoord = RemoveItem(item);
            if (iCoord) Destroy(iCoord.gameObject);
        }

        ItemCoordinator newResult = kManag.CreateNewItemCoord(result, transform.position);
        AddItem(newResult);
    }

    public FoodType GetFoodType()
    {
        if (iCoords.Count == 0) return FoodType.NONE;
        int veg = 0;
        int meat = 0;
        foreach (var i in GetItemList()) {
            if (i.type == FoodType.MEAT) meat += 1;
            if (i.type == FoodType.VEGGIE) veg += 1;
        }
        if (veg + meat == 0) return FoodType.NONE;
        if (veg == 0) return FoodType.MEAT;
        else return FoodType.VEGGIE;
    }

    void PlaySound()
    {
        if (source == null) source = gameObject.GetOrAddComponent<AudioSource>();

        if (!source.isPlaying) AudioManager.instance.PlaySound(actionSoundID, source);
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
