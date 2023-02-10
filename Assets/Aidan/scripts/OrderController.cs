using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [System.Serializable]
    public class OrderInfo
    {
        CharacterName customer;
        Item orderedItem;

        public OrderInfo(CharacterName customer, Item orderedItem)
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
    [HideInInspector] public bool TEMP_HAS_ORDER;

    private void Start()
    {
        GameManager.instance.orderController = this;
    }

    public void Order(Item desire, float patience, CharacterName customerName)
    {
        TEMP_HAS_ORDER = true;
        gameObject.SetActive(true);
        var newOrder = new OrderInfo(customerName, desire);
        AddToOrderList(newOrder, patience, customerName);
    }

    public void CompleteOrder(CharacterName character)
    {
        UIcoord.RemoveItem(character);
    }

    void AddToOrderList(OrderInfo newOrder, float patience, CharacterName customerName)
    {
        UIcoord.AddNew(newOrder.GetItem().GetName(), patience, customerName);
    }

}
