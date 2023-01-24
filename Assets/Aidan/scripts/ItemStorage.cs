using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField] Item storedItem;
    [SerializeField] RestockPromptCoordinator restockCoord;
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

        var coord = KitchenManager.instance.CreateNewItemCoord(storedItem, transform.position);
        KitchenManager.instance.GetChef().HoldNewItem(coord);
    }

    void EnableRestockPrompt()
    {
        stocked = false;
        restockCoord.gameObject.SetActive(true);
    }
}
