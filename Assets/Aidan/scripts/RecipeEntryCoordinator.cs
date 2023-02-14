using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeEntryCoordinator : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI title, ingredients, workSpace, equipment;
    public Recipe recipe;

    [Header("Unread")]
    [SerializeField] Vector2 wiggleSpeedAndStr;
    [SerializeField] Image backing;
    [SerializeField] Color unreadColor;
    Color startingColor;

    bool read;
    Vector3 originalScale;

    public void Init(Recipe _recipe)
    {
        recipe = _recipe;
        title.text = recipe.GetResult().GetName().ToLower();
        workSpace.text = recipe.workSpace.ToString().ToLower();
        img.sprite = recipe.GetResult().GetSprite();

        DisplayList(ingredients, recipe.GetIngredients(), false);
        DisplayList(equipment, recipe.GetEquipment(), true);
    }

    void DisplayList(TextMeshProUGUI tmpro, List<Item> toDisplay, bool addNewline)
    {
        if (toDisplay.Count == 0) { tmpro.text = ""; return; }

        string displayString = "";
        foreach (var i in toDisplay) displayString += i.GetName() + (addNewline ? "\n" : ", ");
        displayString = displayString.Substring(0, displayString.Length - 1);
        if (displayString[displayString.Length-1] == ',') displayString = displayString.Substring(0, displayString.Length - 1);
        tmpro.text = displayString;
    }

    private void Start()
    {
        read = false;
        originalScale = transform.localScale;
        startingColor = backing.color;
    }
    public void Hover()
    {
        read = true;
        transform.localScale = originalScale;
    }

    private void Update()
    { 
        backing.color = read ? startingColor : unreadColor;

        if (read) return;

        var scale = originalScale + Vector3.one * (Mathf.Sin(Time.time * wiggleSpeedAndStr.x) * wiggleSpeedAndStr.y);
        transform.localScale = scale;
    }

}
