using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] Vector2 offset;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.position = Input.mousePosition + (Vector3)offset;
        text.enabled = !string.IsNullOrEmpty(text.text);
    }
}
