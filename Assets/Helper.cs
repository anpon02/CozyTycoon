using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Helper : MonoBehaviour
{
    [SerializeField] Vector3 worldPosTarget;
    [SerializeField] Vector2 xLimits, yLmits;

    private void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(worldPosTarget);
        screenPos.x = Mathf.Clamp(screenPos.x, xLimits.x, xLimits.y);
        screenPos.y = Mathf.Clamp(screenPos.y, yLmits.x, yLmits.y);
        GetComponent<RectTransform>().position = screenPos;
        transform.up = transform.position - worldPosTarget;
    }
}
