using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] Item desiredItem;

    [Header("Debug Tools")]
    [SerializeField] private bool testCustomer;
    [Range(0f, 1f)]
    [SerializeField] private float foodValue;
    private RelationshipStatus status;
    private bool foodOrdered;

    private void Awake() {
        status = GetComponent<RelationshipStatus>();
    }

    private void OnMouseDown()
    {
        Order();
    }

    void Order()
    {
        if (GameManager.instance && GameManager.instance.GetOrderController()) GameManager.instance.GetOrderController().Order(desiredItem);
    }

    /* FOR TESTING PURPOSES */
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H) && testCustomer) {
            if(!foodOrdered) {
                print("Ordered!");
                Order();
                foodOrdered = true;
            }
            else {
                print("Given!");
                status.GiveFood(foodValue);
                foodOrdered = false;
            }
        }
    }
}
