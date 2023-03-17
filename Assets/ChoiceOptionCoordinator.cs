using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceOptionCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quote, unlockText, longText;
    [SerializeField] Image itemImg, speakerPotrait;
    Product product;
    CharacterName character;
    bool DisableCharacters;

    public void Init(KitchenManager.ChoiceData.Option data)
    {
        character = data.character;
        product = data.product;
        itemImg.sprite = product.imgSprite;
        unlockText.text = product.unlocks;
        quote.text = data.quote;
        longText.text = data.quote;

        bool showSpeaker = data.characterSprite != null;
        speakerPotrait.sprite = data.characterSprite;
        speakerPotrait.gameObject.SetActive(showSpeaker);
        quote.gameObject.SetActive(showSpeaker);
        longText.gameObject.SetActive(!showSpeaker);

        DisableCharacters = data.endOfWeek;
    }

    public void OnClick()
    {
        AudioManager.instance.PlaySound(27, gameObject);
        KitchenManager.instance.PurchaseProduct(product, character);
        if (DisableCharacters) KitchenManager.instance.DisableNonChosen();
        Destroy(gameObject);
    }
}
