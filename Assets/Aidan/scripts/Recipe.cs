using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    [HideInInspector] public string name;
    [SerializeField] List<WorkspaceType> workSpaces = new List<WorkspaceType>();
    [SerializeField] List<Item> requiredIngrd = new List<Item>();
    [SerializeField] List<Item> equipment = new List<Item>();
    [SerializeField] Item result;
    [SerializeField] float makeTime;

    public void OnValidate() {
        if (result == null) return;

        name = result.name + " recipe";
    }

    public int GetIngredientCount() {
        return requiredIngrd.Count;
    }

    public List<Item> GetIngredients() {
        return requiredIngrd;
    }
    public Item GetResult() {
        return result;
    }

    public int GetEquipmentCount() {
        return equipment.Count;
    }

    public float GetMakeTime() {
        return makeTime;
    }

    bool validWS(WorkspaceType ws)
    {
        return workSpaces.Contains(ws);
    }

    bool validIngrd(List<Item> candidateIngrds)
    {
        foreach (var reqIngrd in requiredIngrd) {
            if (!reqIngrd.IsPresentInList(candidateIngrds)) return false;
        }
        return true;
    }

    bool ValidEquipment(List<Item> candidateEquip)
    {
        foreach (var i in equipment) {
            if (!i.IsPresentInList(candidateEquip)) return false;
        }
        return true;
    }

    public bool IsValid(out Item _result, out List<Item> consumed, List<Item> candidateIngrds, WorkspaceType ws)
    {
        _result = null;
        consumed = new List<Item>();
        if (!validWS(ws) || !validIngrd(candidateIngrds) || !ValidEquipment(candidateIngrds)) return false;

        _result = result;
        consumed = requiredIngrd;

        return true;
    }

    public bool IsInvalid(List<Item> _ingredients, WorkspaceType ws, int WSroom) {
        if (!workSpaces.Contains(ws)) return true;

        List<Item> stillRequired = new List<Item>(requiredIngrd);
        stillRequired.AddRange(equipment);
        for (int i = 0; i < _ingredients.Count; i++) _ingredients[i].RemoveFromList(stillRequired);
        
        return stillRequired.Count > WSroom;
    }

    
}
