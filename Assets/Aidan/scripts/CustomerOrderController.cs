using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RelationshipStatus))]
public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] Item desiredItem;
    private CustomerStory story;
    private bool foodOrdered;
    private bool hasReceivedFood;

    [Header("Debug Tools")]
    [SerializeField] private bool testCustomer;
    [Range(0f, 1f)]
    [SerializeField] private float foodValue;
    private RelationshipStatus status;

    ThrowingController chef;

    private void Awake() {
        status = GetComponent<RelationshipStatus>();
        story = GetComponent<CustomerStory>();
        hasReceivedFood = false;
    }

    bool CorrectFoodHeld()
    {
        if (!GetChef() || chef.GetHeldItem() == null) return false;
        return chef.GetHeldItem().Equals(desiredItem);
    }

    bool GetChef()
    {
        if (chef) return true;
        if (!KitchenManager.instance) return false;
        chef = KitchenManager.instance.chef;
        return chef != null;
    }

    public void Order()
    {
        AudioManager.instance.PlaySound(7, gameObject);
        foodOrdered = true;
        if (GameManager.instance && GameManager.instance.GetOrderController()) GameManager.instance.GetOrderController().Order(desiredItem);
        CustomerManager.instance.GoToTable(transform);
    }

    public void DeliverFood()
    {
        foodOrdered = true;
        hasReceivedFood = true;
        Item deliveredItem = chef.RemoveHeldItem();
        GameManager.instance.GetOrderController().CompleteOrder(desiredItem);
        status.GiveFood(deliveredItem.quality);
    }

    public bool GetFoodOrdered() {
        return foodOrdered;
    }

    public bool GetHasReceivedFood() {
        return hasReceivedFood;
    }

    public void SetHasReceivedFood(bool received) {
        hasReceivedFood = received;
    }
}
