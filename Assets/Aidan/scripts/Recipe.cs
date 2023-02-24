using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Minigame { NONE, KNIFE, PAN, MIXER, POT, BAKING_TRAY, ROLLING_PIN, COFFEE_MAKER, GRATER }

[System.Serializable]
public class Recipe
{
    [HideInInspector] public string name;
    public WorkspaceType workSpace;
    [SerializeField] List<Item> requiredIngrd = new List<Item>();
    [SerializeField] List<Item> equipment = new List<Item>();
    [SerializeField] Item result;
    [SerializeField] Minigame minigame;
    [HideInInspector] public int index;

    public void OnValidate(int _index) {
        if (result == null) return;
        string ingrds = " ";
        foreach (var i in requiredIngrd) ingrds += i.GetName() + ", ";
        name = _index + ": " + result.name + ":" + ingrds;
        index = _index;
    }

    public int GetIngredientCount() {
        return requiredIngrd.Count;
    }

    public List<Item> GetIngredients() {
        return requiredIngrd;
    }
    public List<Item> GetEquipment()
    {
        return equipment;
    }
    public Item GetResult() {
        return result;
    }

    public int GetEquipmentCount() {
        return equipment.Count;
    }

    public Minigame GetMinigame() {
        return minigame;
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
        if (workSpace != ws || !validIngrd(candidateIngrds) || !ValidEquipment(candidateIngrds)) return false;

        _result = result;
        consumed = requiredIngrd;

        return true;
    }

    public bool IsInvalid(List<Item> _ingredients, WorkspaceType ws, int WSroom) {
        if (workSpace != ws) return true;

        List<Item> stillRequired = new List<Item>(requiredIngrd);
        stillRequired.AddRange(equipment);
        for (int i = 0; i < _ingredients.Count; i++) _ingredients[i].RemoveFromList(stillRequired);
        
        return stillRequired.Count > WSroom;
    }

    
}
