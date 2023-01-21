using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.SetOrderController(this);
    }

    public void Order(Item desire)
    {
        print("a customer wants a " + desire.GetName());
    }
}
