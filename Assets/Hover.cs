using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color normalColor;

    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI txt;

    private void Start()
    {
        img = GetComponent<Image>();
        txt = GetComponent<TextMeshProUGUI>();
        if (img) normalColor = img.color;
        if (txt) normalColor = txt.color;
    }
    public void StartHover()
    {
        if (img) img.color = hoverColor;
        if (txt) txt.color = hoverColor;
    }

    public void EndHover()
    {
        if (img) img.color = normalColor;
        if (txt) txt.color = normalColor;
    }
}
