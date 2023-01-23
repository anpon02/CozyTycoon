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
    
    private Vector2 boxcastOrigin;
    
    private PlayerAnimation anim;
    private string[] directions = {"N", "NW", "W", "SW", 
                                   "S", "SE", "E", "NE"};
    private Vector3[] potentialOrigins;
    private int animLastDir;

    float dir;

    private void Awake() {
        anim = GetComponent<PlayerAnimation>();
        pInputActions = new PlayerInputActions();
        pInputActions.Player.Interact.performed += InteractWithCustomer;

        Vector3[] origins = {(Vector3.up * interactDistance),                       // N
                             ((Vector3.up + Vector3.left) * interactDistance),      // NW
                             (Vector3.left * interactDistance),                     // W
                             ((Vector3.down + Vector3.left) * interactDistance),    // SW
                             (Vector3.down * interactDistance),                     // S
                             ((Vector3.down + Vector3.right) * interactDistance),   // SE
                             (Vector3.right * interactDistance),                    // E
                             ((Vector3.up + Vector3.right) * interactDistance)      // NE
                            };
        potentialOrigins = origins;
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void InteractWithCustomer(InputAction.CallbackContext context) {
        animLastDir = anim.GetLastDir();
        boxcastOrigin = transform.position + potentialOrigins[animLastDir];

        RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, new Vector3(colliderWidth * interactDistance, colliderHeight, 10),
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
        Gizmos.DrawWireCube(boxcastOrigin, 
                            new Vector3(colliderWidth * interactDistance, colliderHeight, 10));
    }
}
