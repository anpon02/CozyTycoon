using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ThrowIndicatorCoordinator : MonoBehaviour
{
    [SerializeField] Vector2 maxScale;
    [SerializeField] Vector2 minScale;
    [SerializeField, Range(0, 1)] float value;

    private void Update()
    {
        SetScale();
        if (Application.isPlaying) FaceMouse();
    }

    public void SetValue(float _value)
    {
        value = _value;
    }

    void SetScale()
    {
        var scale = transform.localScale;
        scale.x = (Mathf.Abs(maxScale.x - minScale.x) * value) + minScale.x;
        scale.y = (Mathf.Abs(maxScale.y - minScale.y) * value) + minScale.y;
        transform.localScale = scale;
    }

    void FaceMouse()
    {
        var mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = dir;
    }
}
