using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<Product> toSell;
    [SerializeField] ShopCoordinator coord;
    [SerializeField] GameObject content, shopButton;
    [SerializeField] int openshopSound;

    private void Start()
    {
        AudioManager.instance.PlaySound(openshopSound, gameObject);
        shopButton.SetActive(false);
    }

    public void ToggleVisible()
    {
        content.SetActive(!gameObject.activeInHierarchy);
        if (content.activeInHierarchy) DisplayProducts();
        shopButton.SetActive(true);
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
