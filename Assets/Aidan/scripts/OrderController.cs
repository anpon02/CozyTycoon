using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [System.Serializable]
    public class OrderInfo
    {
        string customer;
        Item orderedItem;

        public OrderInfo(string customer, Item orderedItem)
        {
            this.customer = customer;
            this.orderedItem = orderedItem;
        }

        public Item GetItem()
        {
            return orderedItem;
        }
    }


    [SerializeField] List<OrderInfo> currentOrders = new List<OrderInfo>();

    private void Start()
    {
        GameManager.instance.SetOrderController(this);
        gameObject.SetActive(false);
    }

    public void Order(Item desire)
    {
        gameObject.SetActive(true);
        print("a customer wants a " + desire.GetName());
    }
}
