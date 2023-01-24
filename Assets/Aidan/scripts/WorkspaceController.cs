using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public enum WorkspaceType {COUNTER, OVEN, STOVE}

[RequireComponent(typeof(WorkspaceCoordinator)), RequireComponent(typeof(AudioSource))]
public class WorkspaceController : MonoBehaviour
{
    [SerializeField] WorkspaceType WorkspaceType;
    [SerializeField] int actionSoundID;
    [SerializeField] ItemCoordinator startingItem; 

    ThrowingController chef;
    WorkspaceCoordinator coord;

    AudioSource source;
    bool readyToComplete;
    Item result;
    List<Item> toRemove;
    float makeTime;


    private void OnMouseDown() {
        if (!SetChef()) return;

        if (chef.IsHoldingItem() || coord.HeldItemCount() == 0) return;

        HaltRecipe();

        var item = coord.removeItem();
        chef.HoldNewItem(item);
    }

    void HaltRecipe()
    {
        if (source != null) source.Stop();
        StopAllCoroutines();
        if (readyToComplete) CompleteRecipe();
        else coord.HideCookPrompt();
    }

    bool SetChef()
    {
        if (KitchenManager.instance) chef = KitchenManager.instance.GetChef();
        return chef != null;
    }

    private void Start()
    {
        coord = GetComponent<WorkspaceCoordinator>();
        if (startingItem) CatchItem(startingItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<ItemCoordinator>();
        if (item != null && !item.GetItem().Equals(KitchenManager.instance.GetChef().GetHeldItem()) ) CatchItem(item);
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
        RecipeManager.instance.CanCombine(out result, out toRemove, out makeTime, coord.GetHeldItems(), this);
        StartCoroutine(CompleteRecipe(result, toRemove, makeTime));
    }

    IEnumerator CompleteRecipe(Item result, List<Item> toRemove, float makeTime) 
    { 
        coord.DisplayPrompt();
        float timeRemaining = makeTime;
        while (timeRemaining >= 0) {
            PlaySound();
            timeRemaining -= Time.deltaTime;
            readyToComplete = coord.SetPromptvalue(1 - (timeRemaining / makeTime));
            if (readyToComplete) coord.previewResult(result.GetSprite());
            yield return new WaitForEndOfFrame();
        }
        
        CompleteRecipe();
    }

    void PlaySound()
    {
        if (source == null) source = gameObject.GetOrAddComponent<AudioSource>();

        if (!source.isPlaying) AudioManager.instance.PlaySound(actionSoundID, source);
    }

    void CompleteRecipe()
    {
        readyToComplete = false;
        foreach (var item in toRemove) coord.removeItem(item);

        float taskCompletionScore = GetTaskCompletionQuality();
        AudioManager.instance.PlaySound((taskCompletionScore > 0.5f) ? 6 : 5);

        CatchItem(KitchenManager.instance.CreateNewItemCoord(result, transform.position, GetResultQuality()));
        coord.HideCookPrompt();
        source.Stop();
    }
    
    float GetResultQuality()
    {
        float previousIngredientAvg = CalcPrevQualityAvg();
        var taskCompletionQuality = GetTaskCompletionQuality();
        if (toRemove.Count == 0 || previousIngredientAvg == -1) return taskCompletionQuality;

        float taskFactor = KitchenManager.instance.GetTaskFactor();
        return (taskCompletionQuality * taskFactor) + (previousIngredientAvg * (1 - taskFactor));
    }

    float GetTaskCompletionQuality()
    {
        float qual = coord.GetCurrentQuality();
        float quality = qual == 0 ? 1 : (qual == 1 ? 0.5f : 0);
        return quality;
    }

    float CalcPrevQualityAvg()
    {
        float previousIngredientAvg = 0;
        int count = 0;
        foreach (var i in toRemove) {
            if (i.GetQuality() != -1) {
                previousIngredientAvg += i.GetQuality();
                count += 1;
            }
        }
        if (count == 0) return -1;
        return previousIngredientAvg /= count;
    }

    public WorkspaceType GetWSType() {
        return WorkspaceType;
    }

    public int GetRoomLeft() {
        return coord.Capacity() - coord.HeldItemCount();
    }
}
