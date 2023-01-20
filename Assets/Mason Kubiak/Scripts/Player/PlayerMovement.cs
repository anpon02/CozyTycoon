using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private PlayerInputActions pInputActions;
    private Rigidbody2D body;

    private void Awake() {
        pInputActions = new PlayerInputActions();
        body = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        GameManager.instance.SetPlayer(gameObject);
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void Move() {
        Vector2 moveInput = pInputActions.Player.Movement.ReadValue<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1);
        body.MovePosition(body.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate() {
        Move();
    }
}
