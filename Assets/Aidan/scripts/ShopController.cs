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
    [SerializeField] bool startEnabled;

    private void Start()
    {
        AudioManager.instance.PlaySound(openshopSound, gameObject);
        shopButton.SetActive(startEnabled);
    }

    public void ToggleVisible()
    {
        content.SetActive(!content.activeInHierarchy);
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
