using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceOptionCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quote, unlockText;
    [SerializeField] Image itemImg, speakerPotrait;
    Product product;
    CharacterName character;

    public void Init(KitchenManager.ChoiceData.Option data)
    {
        character = data.character;
        product = data.product;
        itemImg.sprite = product.imgSprite;
        unlockText.text = product.unlocks;

        speakerPotrait.sprite = data.characterSprite;
        quote.text = data.quote;
    }

    public void OnClick()
    {
        KitchenManager.instance.PurchaseProduct(product, character);
        Destroy(gameObject);
    }
}
