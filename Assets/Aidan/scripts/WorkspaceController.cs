using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkspaceType {CUTTINGBOARD, OVEN, STOVE}

[RequireComponent(typeof(WorkspaceCoordinator))]
public class WorkspaceController : MonoBehaviour
{
    [SerializeField] WorkspaceType WorkspaceType;
    [SerializeField] ThrowingController TEMPCHEF;
    [SerializeField] ItemCoordinator TEMPITEMCOORDPREFAB;
    WorkspaceCoordinator coord;

    private void OnMouseDown() {
        if (TEMPCHEF.IsHoldingItem() || coord.HeldItemCount() == 0) return;

        var item = coord.removeItem();
        item.Show();
        TEMPCHEF.HoldNewItem(item);
    }

    private void Start()
    {
        coord = GetComponent<WorkspaceCoordinator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<ItemCoordinator>();
        if (item != null) CatchItem(item);
    }
    void CatchItem(ItemCoordinator item)
    {
        if (!coord.HasRoom(item.GetItem())) return;

        item.Hide();
        coord.AddItem(item);
        CheckRecipes();
    }

    void CheckRecipes() {
        int options = RecipeManager.instance.numValidOptions(coord.GetHeldItems(), this);
        if (options == 0) return;
        if (options == 1) MakeRecipe();
    }

    void MakeRecipe() {
        var result = default(Item);
        var toRemove = new List<Item>();
        RecipeManager.instance.CanCombine(out result, out toRemove, coord.GetHeldItems(), this);

        foreach (var item in toRemove) coord.removeItem(item);
        CatchItem(CreateNewItemCoord(result));
    }

    ItemCoordinator CreateNewItemCoord(Item item) {
        print("making new item - item: " + item);
        var newGO = Instantiate(TEMPITEMCOORDPREFAB, transform.position, Quaternion.identity);
        var coordScript = newGO.GetComponent<ItemCoordinator>();
        coordScript.SetItem(item);
        coordScript.SetChef(TEMPCHEF);
        return coordScript;
    }

    public WorkspaceType GetWSType() {
        return WorkspaceType;
    }

    public int GetRoomLeft() {
        return coord.Capacity() - coord.HeldItemCount();
    }
}
