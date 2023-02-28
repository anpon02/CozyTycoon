using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color normalColor;

    private void Start()
    {
        normalColor = GetComponent<Image>().color;
    }
    public void StartHover()
    {
        GetComponent<Image>().color = hoverColor;
    }

    public void EndHover()
    {
        GetComponent<Image>().color = normalColor;
    }
}
