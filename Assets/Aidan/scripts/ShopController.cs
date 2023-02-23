using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<Product> toSell;
    [SerializeField] ShopCoordinator coord;
    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        if (gameObject.activeInHierarchy) DisplayProducts();
    }

    public void Remove(Product toRemove)
    {
        
        toSell.Remove(toRemove);
        DisplayProducts();
    }

    void DisplayProducts()
    {
        coord.DisplayProducts(toSell);
    }
}
