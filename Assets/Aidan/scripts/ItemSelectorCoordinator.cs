using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectorCoordinator : MonoBehaviour
{
    [SerializeField] Image displayImg;
    [SerializeField] TextMeshProUGUI itemLabel;
    [SerializeField] ItemStorage storage;
    [SerializeField] GameObject nextItemButton;

    private void Update()
    {
        displayImg.sprite = storage.storedItem.GetSprite();
        itemLabel.text = storage.storedItem.GetName();
        nextItemButton.SetActive(storage.altItems.Count > 0);
    }
}
