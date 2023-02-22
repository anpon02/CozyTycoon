using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCoordinator : MonoBehaviour
{
    [SerializeField] GameObject equipmentParent, specialityParent, listingPrefab;
    [SerializeField] TextMeshProUGUI money;

    public void DisplayProducts(List<Product> toDisplay)
    {
        for (int i = 0; i < equipmentParent.transform.childCount; i++) {
            Destroy(equipmentParent.transform.GetChild(i).gameObject);
        }
        foreach (var product in toDisplay) {
            DisplayProduct(product);
        }
        money.text = "$" + GameManager.instance.wallet.money;
    }
    void DisplayProduct(Product toDisplay)
    {
        var newGO = Instantiate(listingPrefab, equipmentParent.transform);
        var coord = newGO.GetComponent<ShopListingCoordinator>();
        coord.controller = GetComponent<ShopController>();
        coord.Init(toDisplay);
    }
}
