using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int obstacleLayerNum;

    private PlayerInputActions pInputActions;
    private Rigidbody2D body;

    private void Awake() {
        pInputActions = new PlayerInputActions();
        body = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, obstacleLayerNum);
    }

    private void Start() {
        GameManager.instance.player = gameObject;
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
