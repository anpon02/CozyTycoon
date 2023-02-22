using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : MonoBehaviour
{
    private enum InteractableType { ORDER, STORY};

    [SerializeField] private InteractableType type;
    [SerializeField] private Transform player;
    [SerializeField] private CustomerOrderController orderController;
    [SerializeField] private CustomerCoordinator custCoordinator;

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
        if(PauseManager.instance && PauseManager.instance.paused) return;

        // set opacity based on distance if mouse is not hovering over
        if(!mouseOn) {
            playerDistance = Mathf.Clamp(Vector2.Distance(parent.position, player.position), 0.01f, Mathf.Infinity);

            Color sprColor = Color.white;
            sprColor.a = Mathf.Clamp(((KitchenManager.instance.playerReach /2) / playerDistance) - 0.5f, 0, 0.5f);
            sprRenderer.color = sprColor;
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
        mouseOn = false;
        gameObject.SetActive(false);

        if (type == InteractableType.STORY) { 
            custCoordinator.StartStory();
            orderController.storyStarted = true;
            return; 
        }
        if(orderController.alreadyOrdered()) orderController.DeliverFood(); 
        else orderController.Order();  
    }
}
