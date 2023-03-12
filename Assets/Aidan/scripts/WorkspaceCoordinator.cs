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
        GameManager.instance.OnStoreClose.AddListener(UpdateItemDisplay);
    }

    private void Update()
    {
        UpdateItemDisplay();
        if (ws.wsUIcoord.IsMinigameActive()) sRend.color = normalColor;
    }

    private void OnMouseOver()
    {
        sRend.color = hoverColor;
        KitchenManager.instance.hoveredController = ws;
    }

    private void OnMouseExit()
    {
        StartCoroutine(MouseExit());
    }

    IEnumerator MouseExit() {
        for (int i = 0; i < 3; i++) {
            yield return new WaitForEndOfFrame();
        }

        var uiCoord = ws.wsUIcoord;
        if (uiCoord.buttonHovered) yield break;

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

    public void UpdateItemDisplay() 
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
        var iCoordList = ws.GetiCoordList();
        for (int i = 0; i < list.Count; i++) if (list[i].isBigEquipment) list.RemoveAt(i);

        for (int i = 0; i < itemDisplays.Count; i++) {
            if (i >= list.Count) { itemDisplays[i].gameObject.SetActive(false); continue; }

            itemDisplays[i].gameObject.SetActive(!hideItems);
            itemDisplays[i].sprite = list[i].GetSprite();
            itemDisplays[i].transform.GetChild(0).gameObject.SetActive(iCoordList[i].plated);
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
