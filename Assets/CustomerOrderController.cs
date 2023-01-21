using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] Item desiredItem;

    private void OnMouseDown()
    {
        Order();
    }

    void Order()
    {
        if (GameManager.instance && GameManager.instance.GetOrderController()) GameManager.instance.GetOrderController().Order(desiredItem);
    }
}
