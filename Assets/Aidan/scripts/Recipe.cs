using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    [HideInInspector] public string name;
    [SerializeField] List<WorkspaceType> workSpaces = new List<WorkspaceType>();
    [SerializeField] List<Item> ingredients = new List<Item>();
    [SerializeField] List<Item> equipment = new List<Item>();
    [SerializeField] Item result;

    public void OnValidate() {
        if (result == null) return;

        name = result.name + " recipe";
    }

    bool validWS(WorkspaceType ws) {
        return workSpaces.Contains(ws);
    }

    bool validIngrd(List<Item> _ingredients) {
        foreach (var i in _ingredients) {
            if (!ingredients.Contains(i)) return false;
        }
        return true;
    }

    bool ValidEquipment(List<Item> _equpment) {
        foreach (var i in equipment) {
            if (!_equpment.Contains(i)) return false;
        }
        return true;
    }


    public bool IsValid(out Item _result, out List<Item> consumed, List<Item> _ingredients, WorkspaceType ws) {

        _result = null;
        consumed = new List<Item>();
        if (!validWS(ws) || !validIngrd(_ingredients) || !ValidEquipment(_ingredients)) return false;

        _result = result;
        consumed = ingredients;

        return true;
    }

    public int GetIngredientCount() {
        return ingredients.Count;
    }

    public List<Item> GetIngredients() {
        return ingredients;
    }
    public Item GetResult() {
        return result;
    }

    public int GetEquipmentCount() {
        return equipment.Count;
    }

    public bool IsInvalid(List<Item> _ingredients, WorkspaceType ws, int WSroom) {
        if (!workSpaces.Contains(ws)) return false;

        List<Item> stillRequired = new List<Item>(ingredients);
        stillRequired.AddRange(equipment);
        for (int i = 0; i < _ingredients.Count; i++) stillRequired.Remove(_ingredients[i]);

        return stillRequired.Count > WSroom;
    }
}
