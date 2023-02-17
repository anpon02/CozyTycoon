using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Helper : MonoBehaviour
{
    public Vector3 worldPosTarget;
    [SerializeField] Vector2 aPosLimits = new Vector2(5900, 1700), scalar;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        var screenTarget = Camera.main.WorldToScreenPoint(worldPosTarget);

        transform.position = screenTarget;
        var aPos = rect.anchoredPosition;
        aPos.x *= scalar.x;
        aPos.y *= scalar.y;
        aPos.x = Mathf.Clamp(aPos.x, -aPosLimits.x, aPosLimits.x);
        aPos.y = Mathf.Clamp(aPos.y, -aPosLimits.y, aPosLimits.y);
        rect.anchoredPosition = aPos;

        if (Vector3.Distance(transform.position, screenTarget) > 0.01f) transform.up = screenTarget - transform.position;
        else transform.eulerAngles = new Vector3(0, 0 ,-180);
    }
}
