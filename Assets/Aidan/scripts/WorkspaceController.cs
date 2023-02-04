using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WorkspaceType {COUNTER, OVEN, STOVE, SINK}

[RequireComponent(typeof(WorkspaceCoordinator)), RequireComponent(typeof(AudioSource))]
public class WorkspaceController : MonoBehaviour
{
    [SerializeField] WorkspaceType _workspaceType;
    public WorkspaceType workSpaceType { get; private set; }
    [SerializeField] int actionSoundID;
    [SerializeField] ItemCoordinator startingItem;

    ThrowingController chef;
    WorkspaceCoordinator coord;
    KitchenManager kManag;

    AudioSource source;
    Item result;
    List<Item> toRemove;
    float makeTime;

    private void OnValidate()
    {
        workSpaceType = _workspaceType;
    }

    private void Start()
    {
        kManag = KitchenManager.instance;
        coord = GetComponent<WorkspaceCoordinator>();
        if (startingItem) CatchItem(startingItem);
        
    }

    public void HaltRecipe()
    {
        if (source != null) source.Stop();
        StopAllCoroutines();
        coord.HideCookPrompt();
        CheckRecipes();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var iCoord = collision.GetComponent<ItemCoordinator>();
        if (ValidiCood(iCoord)) CatchItem(iCoord);
    }
    bool ValidiCood(ItemCoordinator iCoord)
    {
        if (iCoord == null) return false;
        if (iCoord == kManag.chef.GetHeldiCoord()) return false;
        return true;
    }
    
    void CatchItem(ItemCoordinator iCoord)
    {
        if (!coord.HasRoom(iCoord.GetItem()) || coord.HasItem(iCoord) || iCoord.InWS()) return;
        coord.AddItem(iCoord);
        CheckRecipes();
    }

    void CheckRecipes() {
        int currentOptions = RecipeManager.instance.numValidOptions(coord.GetHeldItems(), this);
        if (currentOptions >= 1) StartRecipe();
    }

    void StartRecipe() {
        RecipeManager.instance.CanCombine(out result, out toRemove, out makeTime, coord.GetHeldItems(), this);
        coord.DisplayPrompt();
        StartCoroutine(StartRecipeRoutine());
    }

    IEnumerator StartRecipeRoutine() 
    {
        float timeRemaining = makeTime;
        while (timeRemaining >= 0) {
            PlaySound();
            timeRemaining -= Time.deltaTime;
            coord.SetPromptvalue(1 - (timeRemaining / makeTime));            
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
        foreach (var item in toRemove) {
            print("item:" + item.GetName());
            var iCoord = coord.removeItem(item);
            if (iCoord) Destroy(iCoord.gameObject);
        }

        float taskCompletionScore = 0.5f;
        AudioManager.instance.PlaySound((taskCompletionScore > 0.5f) ? 6 : 5);
        coord.HideCookPrompt();
        source.Stop();

        ItemCoordinator newResult = kManag.CreateNewItemCoord(result, transform.position, GetResultQuality());
        CatchItem(newResult);
    }
    
    float GetResultQuality()
    {
        float previousIngredientAvg = CalcPrevQualityAvg();
        if (toRemove.Count == 0 || previousIngredientAvg == -1) return 0.5f;

        float taskFactor = kManag.GetTaskFactor();
        return (0.5f * taskFactor) + (previousIngredientAvg * (1 - taskFactor));
    }
    float CalcPrevQualityAvg()
    {
        float previousIngredientAvg = 0;
        int count = 0;
        foreach (var i in toRemove) {
            if (i.quality != -1) {
                previousIngredientAvg += i.quality;
                count += 1;
            }
        }
        if (count == 0) return -1;
        float qual = previousIngredientAvg /= count;
        print("previous item quality: " + qual);
        return qual;
    }

    public int GetRoomLeft() {
        return coord.capacity - coord.HeldItemCount;
    }
}
