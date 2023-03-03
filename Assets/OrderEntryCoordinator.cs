using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderEntryCoordinator : MonoBehaviour
{
    [SerializeField] GameObject expandedParent;
    [SerializeField] TextMeshProUGUI mainText, sideText;
    [SerializeField] Image portrait;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] Vector2 prefferedSizes = new Vector2();
    bool expanded;
    string main, side;
    public OrderUICoordinator uiCoord;

    private void Start()
    {
        uiCoord.orderEntryCoords.Add(this);
    }

    public void Init(string _main, string _side, Sprite portrait)
    {
        //main = 
    }

    public void Toggle()
    {
        expanded = !expanded;
        if (expanded) uiCoord.HideAllOrders();
        expanded = true;
    }

    public void Hide()
    {
        expanded = false;
    }

    private void Update()
    {
        layoutElement.preferredWidth = expanded ? prefferedSizes.y : prefferedSizes.x;
        expandedParent.SetActive(expanded);
    }
}
