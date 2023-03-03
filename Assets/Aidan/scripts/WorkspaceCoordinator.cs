using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Cecil.Cil;
using UnityEditor;

public class WorkspaceCoordinator : MonoBehaviour
{
    SpriteRenderer sRend;
    [SerializeField] Color hoverColor, normalColor = Color.white;
    WorkspaceController ws;
    
    [SerializeField] SpriteRenderer bigDisplay;
    [SerializeField] List<SpriteRenderer> itemDisplays;
    ItemCoordinator bigItem;
    [HideInInspector] public bool hideItems;
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
        foreach (var d in itemDisplays) d.gameObject.SetActive(false);
        foreach (var d in itemDisplays) d.enabled = true;
    }

    void ShowSprites()
    {
        if (bigDisplay) {
            bigDisplay.enabled = true;
        }
        foreach (var d in itemDisplays) d.gameObject.SetActive(true);
    }

    void UpdateItemDisplay()
    {
        if (!ws) ws = GetComponent<WorkspaceController>();

        bigDisplay.sprite = null;
        foreach (var i in ws.GetItemList()) if (i.isBigEquipment) bigDisplay.sprite = i.GetSprite();
        bigDisplay.enabled = bigDisplay.sprite != null;

        UpdateBigItemDisplay();
        UpdateNormalItemDisplays();
        
    }

    void UpdateNormalItemDisplays()
    {
        var list = ws.GetItemList();
        for (int i = 0; i < list.Count; i++) if (list[i].isBigEquipment) list.RemoveAt(i);

        for (int i = 0; i < itemDisplays.Count; i++) {
            if (i >= list.Count) { itemDisplays[i].gameObject.SetActive(false); continue; }

            itemDisplays[i].gameObject.SetActive(!hideItems);
            itemDisplays[i].sprite = list[i].GetSprite();
        }
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
