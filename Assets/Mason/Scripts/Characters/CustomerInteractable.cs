using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : MonoBehaviour
{
    private enum InteractableType { Order, Story};

    [SerializeField] private InteractableType type;
    [SerializeField] private Transform player;
    [SerializeField] private CustomerOrderController orderController;
    [SerializeField] private CustomerStory custStory;

    private SpriteRenderer sprRenderer;
    private Transform parent;
    private float playerDistance;
    private bool mouseOn;

    private void Awake() {
        sprRenderer = GetComponent<SpriteRenderer>();
        parent = transform.parent;
        mouseOn = false;
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.GetPaused()) return;

        // set opacity based on distance if mouse is not hovering over
        if(!mouseOn) {
            playerDistance = Mathf.Clamp(Vector2.Distance(parent.position, player.position), 0.01f, Mathf.Infinity);

            Color sprColor = Color.white;
            sprColor.a = Mathf.Clamp(((KitchenManager.instance.playerReach /2) / playerDistance) - 0.5f, 0, 0.5f);
            sprRenderer.color = sprColor;

            // ensure object is always clickable
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void OnMouseOver() {
        sprRenderer.color = Color.white;
        mouseOn = true;
    }

    private void OnMouseExit() {
        mouseOn = false;
    }
    
    private void OnMouseDown() {
        if(playerDistance <= KitchenManager.instance.playerReach) {
            // if order
            if(type == InteractableType.Order) {
                // order or deliver food
                if(!orderController.GetFoodOrdered()) {
                    print("ordered!");
                    orderController.Order();
                }
                else {
                    print("delivered!");
                    orderController.DeliverFood();
                }
            }
            // if story
            else {
                print("story!");
                custStory.StartStory();
            }

            mouseOn = false;
            gameObject.SetActive(false);
        }
    }
}
