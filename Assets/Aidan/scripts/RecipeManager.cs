using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;
    void Awake() { instance = this; }

    [SerializeField] List<Recipe> allRecipes = new List<Recipe>();

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

    private void OnValidate() {
        foreach (var r in allRecipes) {
            r.OnValidate();
        }
    }

    public List<Item> GetPossibleFutureRecipes(List<Item> candidateIngrds, WorkspaceType ws, int wsRoom) {
        List<Item> results = new List<Item>();
        foreach (var r in allRecipes) if (!r.IsInvalid(candidateIngrds, ws, wsRoom)) results.Add(r.GetResult());
        return results;
    }
}
