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
    List<Product> affordable = new List<Product>();
    int notifiedNum;

    public void PlaySound()
    {
        AudioManager.instance.PlaySound(openshopSound, gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseShop();
        NotifyAfford();
    }

    void NotifyAfford()
    {
        int num = affordable.Count;
        foreach (var p in toSell) if (p.quantity == -1 && !affordable.Contains(p) && p.price <= GameManager.instance.wallet.money) affordable.Add(p);
        if (num < affordable.Count) GameManager.instance.Notify(callback: OpenShop);
    }

    private void Start()
    {
        shopButton.SetActive(startEnabled);
    }

    public void ToggleVisible()
    {
        if (content.activeInHierarchy) CloseShop();
        else OpenShop();
    }

    public void OpenShop()
    {
        if (content.activeInHierarchy) return;
        
        content.SetActive(true);
        DisplayProducts();
        shopButton.SetActive(true);
        PauseManager.instance.numOpenMenus += 1;
    }

    public void CloseShop()
    {
        if (!content.activeInHierarchy) return;
        
        content.SetActive(false);
        PauseManager.instance.numOpenMenus -= 1;
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
