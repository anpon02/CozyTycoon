using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField] Item storedItem;
    [SerializeField] RestockPromptCoordinator restockCoord;
    [SerializeField, Range(0, 1)] float itemStartQuality = 0;
    [SerializeField] string toolTip;

    bool stocked = true;

    public void Stock()
    {
        stocked = true;
    }

    private void Start()
    {
        GameManager.instance.OnStoreClose.AddListener(EnableRestockPrompt);
    }

    private void OnMouseDown()
    {
        if (!KitchenManager.instance || !stocked) return;

        var chef = KitchenManager.instance.GetChef();
        if (chef == null) return;
        if (chef.GetHeldItem() != null) return;

        var coord = KitchenManager.instance.CreateNewItemCoord(storedItem, transform.position, itemStartQuality);
        KitchenManager.instance.GetChef().HoldNewItem(coord);
    }

    void EnableRestockPrompt()
    {
        stocked = false;
        restockCoord.gameObject.SetActive(true);
    }

    private void OnMouseEnter()
    {
        KitchenManager.instance.ttCoord.Display(toolTip);
    }
    private void OnMouseExit()
    {
        KitchenManager.instance.ttCoord.ClearText(toolTip);
    }
}
