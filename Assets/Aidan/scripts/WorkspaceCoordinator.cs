using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkspaceCoordinator : MonoBehaviour
{
    SpriteRenderer sRend;
    [SerializeField] Color hoverColor, normalColor = Color.white;
    WorkspaceController ws;
    
    [SerializeField] SpriteRenderer bigDisplay;
    [SerializeField] SpriteRenderer itemDisplay;
    ItemCoordinator bigItem;

    private void Start()
    {
        ws = GetComponent<WorkspaceController>();
        sRend = GetComponent<SpriteRenderer>();
        StopAllCoroutines();
        HideSprites();
    }

    private void Update()
    {
        UpdateItemDisplay();
        if (ws.wsUIcoord.IsMinigameActive()) sRend.color = normalColor;
    }

    private void OnMouseEnter()
    {
        sRend.color = hoverColor;
        KitchenManager.instance.hoveredController = ws;
    }

    private void OnMouseExit()
    {
        sRend.color = normalColor;
        if (KitchenManager.instance.hoveredController == ws) KitchenManager.instance.hoveredController = null;
    }

    void HideSprites()
    {
        if (bigDisplay) {
            bigDisplay.enabled = false;
        }
    }

    void ShowSprites()
    {
        if (bigDisplay) {
            bigDisplay.enabled = true;
        }
    }

    bool roomForBigEquipment()
    {
        return bigItem == null && bigDisplay != null;
    }
    void UpdateItemDisplay()
    {
        if (!ws) ws = GetComponent<WorkspaceController>();

        bigDisplay.sprite = null;
        foreach (var i in ws.GetItemList()) if (i.isBigEquipment) bigDisplay.sprite = i.GetSprite();
        bigDisplay.enabled = bigDisplay.sprite != null;

        var type = ws.GetFoodType();
        itemDisplay.gameObject.SetActive(type != FoodType.NONE);
        itemDisplay.sprite = type == FoodType.MEAT ? KitchenManager.instance.genericMeat : KitchenManager.instance.genericVeggies;
        UpdateBigItemDisplay();
    }

    void UpdateBigItemDisplay()
    {
        if (!bigDisplay || !bigItem) return;
        bigItem.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;
        StopAllCoroutines();
        ShowSprites();
        StartCoroutine(HideSpritesAfterDelay());
    }

    IEnumerator HideSpritesAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        HideSprites();
    }
}
