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
    [SerializeField] OrderUICoordinator UIcoord;

    private void Start()
    {
        GameManager.instance.SetOrderController(this);
    }

    public void Order(Item desire, string customerName = "UNNAMED CUSTOMER")
    {
        gameObject.SetActive(true);
        var newOrder = new OrderInfo(customerName, desire);
        AddToOrderList(newOrder);
    }

    public void CompleteOrder(Item orderedItem)
    {
        UIcoord.RemoveItem(orderedItem.GetName());
    }

    void AddToOrderList(OrderInfo newOrder)
    {
        UIcoord.AddNew(newOrder.GetItem().GetName());
    }

}
