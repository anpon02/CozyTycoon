using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkspaceType {CUTTINGBOARD, OVEN, STOVE}

[RequireComponent(typeof(WorkspaceCoordinator))]
public class WorkspaceController : MonoBehaviour
{
    [SerializeField] WorkspaceType WorkspaceType;
    ThrowingController chef;
    WorkspaceCoordinator coord;

    private void OnMouseDown() {
        if (!SetChef()) return;

        if (chef.IsHoldingItem() || coord.HeldItemCount() == 0) return;

        var item = coord.removeItem();
        chef.HoldNewItem(item);
    }

    bool SetChef()
    {
        if (KitchenManager.instance) chef = KitchenManager.instance.GetChef();
        return chef != null;
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
        int currentOptions = RecipeManager.instance.numValidOptions(coord.GetHeldItems(), this);
        if (currentOptions == 1) MakeRecipe();
    }

    void MakeRecipe() {
        Item result;
        List<Item> toRemove;
        float makeTime;
        RecipeManager.instance.CanCombine(out result, out toRemove, out makeTime, coord.GetHeldItems(), this);

        StartCoroutine(CompleteRecipe(result, toRemove, makeTime));
    }

    IEnumerator CompleteRecipe(Item result, List<Item> toRemove, float makeTime) {
        coord.DisplayPrompt();
        float timeRemaining = makeTime;
        while (timeRemaining >= 0) {
            timeRemaining -= Time.deltaTime;
            coord.SetPromptvalue(1 - (timeRemaining / makeTime));
            yield return new WaitForEndOfFrame();
        }
        coord.HideCookPrompt();

        foreach (var item in toRemove) coord.removeItem(item);
        CatchItem(KitchenManager.instance.CreateNewItemCoord(result, transform.position));
    }
    

    public WorkspaceType GetWSType() {
        return WorkspaceType;
    }

    public int GetRoomLeft() {
        return coord.Capacity() - coord.HeldItemCount();
    }
}
