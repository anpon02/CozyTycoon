using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePromptCoordinator : MonoBehaviour
{
    [SerializeField] ItemStorage storage;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI detailText;

    private void Update()
    {
        priceText.color = GameManager.instance.wallet.money < storage.upgradeCost ? Color.red : Color.black;
    }

    public void OnEnable()
    {
        priceText.text = "$" + storage.upgradeCost;
        mainText.text = "click to upgrade " + storage.upgradeName;
        detailText.text = storage.upgradeDetails;
    }
 }
