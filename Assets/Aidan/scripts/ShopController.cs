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
    [SerializeField] int purchaseSound;
    [SerializeField] bool startEnabled;

    public void PlaySound()
    {
        AudioManager.instance.PlaySound(openshopSound, gameObject);
    }

    private void Start()
    {
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
        AudioManager.instance.PlaySound(purchaseSound, gameObject);
        toSell.Remove(toRemove);
        DisplayProducts();
    }

    void DisplayProducts()
    {
        coord.DisplayProducts(toSell);
    }
}
