using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [SerializeField] int completeorderSound;
    [System.Serializable]
    public class OrderInfo
    {
        public CharacterName customer;
        public Item main, side;

        public OrderInfo(CharacterName customer, Item main, Item side)
        {
            this.customer = customer;
            this.main = main;
            this.side = side;
        }

        public string GetSideName()
        {
            if (side) return side.GetName();
            return "";
        }
    }
    [SerializeField] OrderUICoordinator UIcoord;
    [HideInInspector] public bool TEMP_HAS_ORDER, completedOrder;

    private void Start()
    {
        GameManager.instance.orderController = this;
    }

    public void Order(Item mainItem, CharacterName customerName, Item sideItem = null)
    {
        TEMP_HAS_ORDER = true;
        gameObject.SetActive(true);
        var newOrder = new OrderInfo(customerName, mainItem, sideItem);
        AddToOrderList(newOrder);
    }

    public void CompleteOrder(CharacterName character)
    {
        AudioManager.instance.PlaySound(completeorderSound, gameObject);
        completedOrder = true;
        UIcoord.RemoveItem(character);
    }

    void AddToOrderList(OrderInfo newOrder)
    {
        UIcoord.AddNew(newOrder.main.GetName(), newOrder.GetSideName(), newOrder.customer);
    }

}
