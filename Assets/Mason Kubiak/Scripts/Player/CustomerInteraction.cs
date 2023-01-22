using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerInteraction : MonoBehaviour
{
    private PlayerInputActions pInputActions;
    [SerializeField] private float interactDistance;
    [SerializeField] private float colliderWidth;
    [SerializeField] private float colliderHeight;
    [SerializeField] private LayerMask customerLayer;
    float dir;

    private void Awake() {
        pInputActions = new PlayerInputActions();
        print(pInputActions.Player.Interact);
        pInputActions.Player.Interact.performed += InteractWithCustomer;
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void PingForCustomer(InputAction.CallbackContext context) {
        dir = transform.rotation.x == 0 ? - 1 : 1;
    }

    private void InteractWithCustomer(InputAction.CallbackContext context) {
        dir = transform.rotation.x == 0 ? -1 : 1;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (transform.right * interactDistance * dir),
                                             new Vector3(colliderWidth * interactDistance, colliderHeight, 10),
                                             0, Vector2.left, 0, customerLayer);
        if(hit.collider != null) {
            CustomerOrderController cust = hit.collider.GetComponent<CustomerOrderController>();
            if(cust.GetFoodOrdered())
                cust.Order();
            else
                cust.DeliverFood();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        dir = transform.rotation.x == 0 ? -1 : 1;
        Gizmos.DrawWireCube(transform.position + (transform.right * interactDistance * dir), 
                            new Vector3(colliderWidth * interactDistance, colliderHeight, 10));
    }
}
