using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;
    void Awake() { instance = this; }

    [SerializeField] List<Recipe> allRecipes = new List<Recipe>();
    [HideInInspector] public List<Recipe> unlockedRecipes = new List<Recipe>();
    List<Item> unlockedResults = new List<Item>();
    public List<Item> Menu { get { return GetMenu(); } }
    KitchenManager kMan;
    [HideInInspector] public bool tutorialRecipeLimit;

    public bool CanCombine(out Item result, out List<Item> consumed, List<Item> _ingredients, WorkspaceController ws) {
        result = null;
        consumed = new List<Item>();
        List<Recipe> options = GetValidRecipes(_ingredients, ws);
        if (options.Count == 0) return false;

        Recipe best = options[0];
        for (int i = 0; i < options.Count; i++) {
            int countNew = options[i].GetIngredientCount();
            int countOld = best.GetIngredientCount();
            if (countNew > countOld) best = options[i];
            else if (countNew == countOld && options[i].GetEquipmentCount() > best.GetEquipmentCount()) best = options[i];
        }
        consumed = best.GetIngredients();
        result = best.GetResult();
        return true;
    }

    public List<Recipe> GetValidRecipes(List<Item> _ingredients, WorkspaceController ws) {
        var valid = new List<Recipe>();
        foreach (var r in allRecipes) if (r.IsValid(out var result, out var consumer, _ingredients, ws.workSpaceType)) valid.Add(r);
        return valid;
    }

    public int numValidOptions(List<Item> _ingredients, WorkspaceController ws) {
        return GetValidRecipes(_ingredients, ws).Count;
    }

    public List<Item> GetPossibleFutureRecipes(List<Item> candidateIngrds, WorkspaceType ws, int wsRoom)
    {
        List<Item> results = new List<Item>();
        foreach (var r in allRecipes) if (!r.IsInvalid(candidateIngrds, ws, wsRoom)) results.Add(r.GetResult());
        return results;
    }

    List<Item> GetMenu()
    {
        var list = new List<Item>();
        foreach (var i in unlockedResults) if (i.menuItem) list.Add(i);
        return list;
    }

    private void Start()
    {
        kMan = KitchenManager.instance;
    }

    private void Update()
    {
        if (tutorialRecipeLimit) return;

        CheckForNewRecipes();
    }

    void CheckForNewRecipes()
    {
        if (allRecipes.Count == unlockedRecipes.Count) return;

        List<Recipe> newUnlocked = GetNewUnlock();
        if (newUnlocked.Count == 0) return;

        AddNewRecipes(newUnlocked);
    }

    List<Recipe> GetNewUnlock()
    {
        var lockedRecipes = allRecipes.Except(unlockedRecipes);
        List<Recipe> newUnlocked = new List<Recipe>();
        int unlockedThisTime;
        do {
            unlockedThisTime = 0;
            foreach (var r in lockedRecipes) {
                if (Possible(r) && !newUnlocked.Contains(r)) { newUnlocked.Add(r); unlockedThisTime += 1; }
            }
        } while (unlockedThisTime > 0);

        return newUnlocked;
    }

    bool Possible(Recipe recipe)
    {
        var equipment = kMan.unlockedEquipment;
        foreach (var e in recipe.GetEquipment()) if (!e.IsPresentInList(equipment)) return false;
        foreach (var i in recipe.GetIngredients()) if (!i.IsPresentInList(unlockedResults) && HasRecipe(i)) return false;

        return true;
    }

    bool HasRecipe(Item toCheck)
    {
        foreach (var recipe in allRecipes) if (recipe.GetResult().Equals(toCheck)) return true;
        return false;
    }

    void AddNewRecipes(List<Recipe> newUnlocked)
    {
        var results = new List<Item>();
        foreach (var r in newUnlocked) results.Add(r.GetResult());

        unlockedResults.AddRange(results);
        unlockedRecipes.AddRange(newUnlocked);

        AddIngredients(newUnlocked);
    }

    void AddIngredients(List<Recipe> newUnlocked)
    {
        foreach (var r in newUnlocked) {
            UnlockAllIngredients(r);
        }
    }

    void UnlockAllIngredients(Recipe r)
    {
        foreach (var i in r.GetIngredients()) {
            if (!i.IsPresentInList(kMan.unlockedIngredients) && !HasRecipe(i))
                kMan.EnableIngredient(i);
        }
    }

    void OnValidate() {
        foreach (var r in allRecipes) {
            r.OnValidate();
        }
    }

}
