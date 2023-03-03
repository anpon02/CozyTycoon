using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractableCoordinator : MonoBehaviour
{
    [SerializeField] private CustomerInteractable forkKnife;
    [SerializeField] private CustomerInteractable story;

    private CustomerMovement movement;
    private CustomerOrderController orderController;
    private CustomerCoordinator custCoordinator;

    private void Awake() {
        movement = GetComponent<CustomerMovement>();
        orderController = GetComponentInChildren<CustomerOrderController>();
        custCoordinator = GetComponent<CustomerCoordinator>();
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.paused) return;

        if(!movement.IsMoving() && !orderController.GetHasReceivedFood()) {
            forkKnife.gameObject.SetActive(true);
        }
        if(!movement.IsMoving() && movement.GetCurrentTable() && !custCoordinator.GetStorySaid()) {
            story.gameObject.SetActive(true);
        }
        if(movement.IsMoving()) {
            forkKnife.gameObject.SetActive(false);
            story.gameObject.SetActive(false);
        }
    }
}
