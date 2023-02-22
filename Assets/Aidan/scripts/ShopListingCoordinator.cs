using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopListingCoordinator : MonoBehaviour
{
    [SerializeField] Image itemImg;
    [SerializeField] TextMeshProUGUI listingName, description, unlock, price, specialityNote;
    public Product product;
    [HideInInspector] public ShopController controller;
    [SerializeField] bool specailityItem;
    [SerializeField] GameObject unlockHeader, unlockUnderline;

    private void Start()
    {
        if (product) Init(product);
    }

    public void Init(Product p)
    {
        specailityItem = p.quantity != -1;

        product = p;
        itemImg.sprite = p.imgSprite;
        listingName.text = p.productName + (specailityItem ? " x" + p.quantity : "");
        description.text = p.description;
        unlock.text = p.unlocks;
        price.text = "$" + p.price;
        specialityNote.gameObject.SetActive(false);
        
        if (p.quantity == -1) return;
        unlockHeader.SetActive(false);
        unlockUnderline.SetActive(false);
        unlock.gameObject.SetActive(false);
        specialityNote.text = p.unlocks;
        specialityNote.gameObject.SetActive(true);
    }

    private void Update()
    {
        price.color = product.price > GameManager.instance.wallet.money ? Color.red : Color.black;
    }

    public void BuyItem()
    {
        if (product.price > GameManager.instance.wallet.money) return;
        GameManager.instance.wallet.money -= product.price;

        KitchenManager.instance.PurchaseProduct(product, !specailityItem);
        if (!specailityItem) controller.Remove(product);
    }
}
