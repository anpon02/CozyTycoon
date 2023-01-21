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
    [SerializeField] Color completeCol;

    [SerializeField] bool TESTBOOL;

    string itemName;

    private void Update()
    {
        if (TESTBOOL) {
            TESTBOOL = false;
            MarkComplete();
        }
    }


    public void Init(string _text)
    {
        checkBox.sprite = uncheckedBox;
        label.text = _text;
        itemName = _text;
    }

    public void MarkComplete()
    {
        checkBox.sprite = checkedBox;
        label.color = checkBox.color = completeCol;
        label.fontStyle = FontStyles.Italic;
    }

    public string GetItemName()
    {
        return itemName;
    }
}
