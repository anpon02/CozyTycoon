using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderListItemCoordinator : MonoBehaviour
{
    [SerializeField] Image checkBox;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Sprite uncheckedBox;
    [SerializeField] Sprite checkedBox;

    public void MarkComplete()
    {
        checkBox.sprite = checkedBox;
    }
}
