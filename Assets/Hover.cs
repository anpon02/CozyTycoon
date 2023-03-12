using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color normalColor = Color.black;

    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI txt;

    private void OnEnable() {
        
        if (normalColor == Color.black) Start();
        EndHover();
    }

    private void Start()
    {
        if (img == null) img = GetComponent<Image>();
        if (txt == null) txt = GetComponent<TextMeshProUGUI>();
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
