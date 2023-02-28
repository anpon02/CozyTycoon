using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;
    void Awake() { instance = this; }

    [SerializeField] List<Recipe> allRecipes = new List<Recipe>();

    [Header("Runtime")]
    public List<Recipe> unlockedRecipes = new List<Recipe>();
    List<Item> unlockedResults = new List<Item>();
    public List<Item> Menu { get { return GetMenu(); } }
    public List<Item> Sides { get { return GetSides(); } }
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

    List<Item> GetSides()
    {
        var list = new List<Item>();
        foreach (var i in unlockedResults) if (i.side) list.Add(i);
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
        //if (allRecipes.Count == unlockedRecipes.Count) return;

        List<Recipe> newUnlocked = GetNewUnlock();
        if (newUnlocked.Count == 0) return;

        AddNewRecipes(newUnlocked);
    }

    List<Recipe> GetNewUnlock()
    {
        List<Recipe> lockedRecipes = SubtractList(allRecipes, unlockedRecipes);

        List<Recipe> newUnlocked = new List<Recipe>();
        var lockedMeals = GetLockedMeals(lockedRecipes);
        foreach (var l in lockedMeals) {
            var possible = CheckIntermediates(l);
            if (possible != null) newUnlocked.AddRange(possible);
        }

        return newUnlocked;
    }

    List<Recipe> SubtractList(List<Recipe> list1, List<Recipe> list2)
    {
        List<Recipe> list = new List<Recipe>();
        foreach (var l in list1) if (!list2.Contains(l)) list.Add(l);
        return list;
    }

    List<Item> SubtractList(List<Item> list1, List<Item> list2)
    {
        List<Item> list = new List<Item>();
        foreach (var l in list1) if (!list2.Contains(l)) list.Add(l);
        return list;
    }

    List<Recipe> CheckIntermediates(Recipe meal)
    {
        List<Recipe> intermediates = GetAllIntermediates(meal);
        List<Recipe> newUnlocked = new List<Recipe>();
        int unlockedThisTime;
        do {
            unlockedThisTime = 0;
            foreach (var r in intermediates) {
                if (Possible(r, newUnlocked) && !newUnlocked.Contains(r)) { newUnlocked.Add(r); unlockedThisTime += 1; }
            }
        } while (unlockedThisTime > 0);

        string inter = "";
        foreach (var i in intermediates) inter += i.GetResult().GetName() + ", ";
        string unlocked = "";
        foreach (var i in newUnlocked) unlocked += i.GetResult().GetName() + ", ";
        //print("checking intermediates for: " + meal.GetResult().GetName() + ", intermediates: " + inter + ", unlcoked intermediates: " + unlocked);

        if (newUnlocked.Count == intermediates.Count) return newUnlocked;
        return null;
    }

    List<Recipe> GetAllIntermediates(Recipe top)
    {
        List<Recipe> list = new List<Recipe>();
        list.Add(top);
        foreach (var i in top.GetIngredients()) {
            if (HasRecipe(i)) list.AddRange(GetAllIntermediates(GetRecipeForItem(i)));
        }
        return list;
    }

    List<Recipe> GetLockedMeals(List<Recipe> lockedRecipes)
    {
        var list = new List<Recipe>();
        foreach (var r in lockedRecipes) if (r.GetResult().menuItem) list.Add(r);
        return list;
    }

    Recipe GetRecipeForItem(Item item)
    {
        foreach (var recipe in allRecipes) if (recipe.GetResult().Equals(item)) return recipe;
        return null;
    }

    bool Possible(Recipe recipe, List<Recipe> additionalUnlocked)
    {
        List<Item> totalUnlocked = new List<Item>();
        foreach (var r in additionalUnlocked) totalUnlocked.Add(r.GetResult());
        totalUnlocked.AddRange(unlockedResults);

        var equipment = kMan.unlockedEquipment;
        foreach (var e in recipe.GetEquipment()) if (!e.IsPresentInList(equipment)) return false;
        foreach (var i in recipe.GetIngredients()) if (!i.IsPresentInList(totalUnlocked) && HasRecipe(i)) return false;

        return true;
    }

    string PrintList(string preface, List<Item> list)
    {
        string s = preface;
        foreach (var i in list) s += i.GetName() + ", ";
        return s;
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

        foreach (var r in results) if (!unlockedResults.Contains(r)) unlockedResults.Add(r);
        foreach (var u in newUnlocked) if (!unlockedRecipes.Contains(u)) unlockedRecipes.Add(u);

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
        for (int i = 0; i < allRecipes.Count; i++) {
            allRecipes[i].OnValidate(i);
        }
    }

}
