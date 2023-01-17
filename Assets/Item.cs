using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ThrowingController chef;

    private void OnMouseDown()
    {
        chef.HoldNewItem(gameObject);
    }
}
