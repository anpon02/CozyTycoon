using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickToCall : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    public UnityEvent OnClick;

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = hoverColor;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnMouseDown()
    {
        OnClick.Invoke();
    }
}
