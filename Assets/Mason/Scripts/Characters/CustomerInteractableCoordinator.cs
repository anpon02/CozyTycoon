using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractableCoordinator : MonoBehaviour
{
    [SerializeField] private CustomerInteractable forkKnife;
    [SerializeField] private CustomerInteractable story;

    private CustomerMovement movement;
    private CustomerOrderController orderController;
    private CustomerStory custStory;

    private void Awake() {
        movement = GetComponent<CustomerMovement>();
        orderController = GetComponentInChildren<CustomerOrderController>();
        custStory = GetComponentInChildren<CustomerStory>();
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.GetPaused()) return;

        if(!movement.IsMoving() && !orderController.GetHasReceivedFood()) {
            forkKnife.gameObject.SetActive(true);
        }
        if(!movement.IsMoving() && !custStory.GetStorySaid()) {
            story.gameObject.SetActive(true);
        }
        if(movement.IsMoving()) {
            forkKnife.gameObject.SetActive(false);
            story.gameObject.SetActive(false);
        }
    }
}
