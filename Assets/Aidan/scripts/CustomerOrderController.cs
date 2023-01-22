using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RelationshipStatus))]
public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] Item desiredItem;
    private bool foodOrdered;

    [Header("Debug Tools")]
    [SerializeField] private bool testCustomer;
    [Range(0f, 1f)]
    [SerializeField] private float foodValue;
    private RelationshipStatus status;

    ThrowingController chef;

    private void Awake() {
        status = GetComponent<RelationshipStatus>();
    }

    private void OnMouseDown()
    {
        if (!foodOrdered) Order();
        else if (CorrectFoodHeld()) DeliverFood();
    }

    bool CorrectFoodHeld()
    {
        if (!GetChef()) return false;
        return chef.GetHeldItem().Equals(desiredItem);
    }

    bool GetChef()
    {
        if (chef) return true;
        if (!KitchenManager.instance) return false;
        chef = KitchenManager.instance.GetChef();
        return chef != null;
    }

    public void Order()
    {
        foodOrdered = true;
        if (GameManager.instance && GameManager.instance.GetOrderController()) GameManager.instance.GetOrderController().Order(desiredItem);
    }

    public void DeliverFood()
    {
        foodOrdered = true;
        Item deliveredItem = chef.RemoveHeldItem();
        GameManager.instance.GetOrderController().CompleteOrder(desiredItem);
        status.GiveFood(deliveredItem.GetQuality());
    }

    public bool GetFoodOrdered() {
        return foodOrdered;
    }


    /* FOR TESTING PURPOSES */
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H) && testCustomer) {
            if(!foodOrdered) {
                print("Ordered!");
                Order();
            }
            else {
                print("Given!");
                status.GiveFood(foodValue);
            }
        }
    }
}
