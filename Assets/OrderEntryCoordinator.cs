using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderEntryCoordinator : MonoBehaviour
{
    [SerializeField] GameObject expandedParent, plus;
    [SerializeField] TextMeshProUGUI mainText, sideText;
    [SerializeField] Image portrait;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] Vector2 prefferedSizes = new Vector2();
    bool expanded;
    string main, side;
    public OrderUICoordinator uiCoord;
    public CharacterName character;

    private void Start()
    {
        uiCoord.orderEntryCoords.Add(this);
    }

    public void Init(string _main, string _side, Sprite _portrait, CharacterName _character)
    {
        character = _character;
        mainText.text = _main;
        sideText.text = _side;
        portrait.sprite = _portrait;
        plus.SetActive(!string.IsNullOrEmpty(_side));
        expanded = false;
        Toggle();
    }

    public void Toggle()
    {
        expanded = !expanded;
        if (!expanded) return;

        uiCoord.HideAllOrders();
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
