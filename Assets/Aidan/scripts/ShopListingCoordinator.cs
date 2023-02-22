using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopListingCoordinator : MonoBehaviour
{
    [SerializeField] Image itemImg;
    [SerializeField] TextMeshProUGUI listingName, description, unlock, price;
    public Product product;
    [HideInInspector] public ShopController controller;
    [SerializeField] bool specailityItem;

    private void Start()
    {
        if (product) Init(product);
    }

    public void Init(Product p)
    {
        product = p;
        itemImg.sprite = p.imgSprite;
        listingName.text = p.productName + (specailityItem ? " x" + p.quantity : "");
        description.text = p.description;
        unlock.text = p.unlocks;
        price.text = "$" + p.price;
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
