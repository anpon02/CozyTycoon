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
    [HideInInspector] public bool buttonVisible;

    private void Awake()
    {
        buttonVisible = true;
    }

    private void Update()
    {
        displayImg.sprite = storage.currentItem.GetSprite();
        itemLabel.text = storage.currentItem.GetName();
        nextItemButton.SetActive(buttonVisible && storage.numItems > 1);
    }
}
