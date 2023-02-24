using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int obstacleLayerNum;

    public Vector2 moveInput { get; private set; }
    public bool isometricMovement;
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

    private Vector2 IsometricConversion(Vector2 position) {
        Vector2 isoVec = new Vector2();
        isoVec.x = position.x - position.y;
        isoVec.y = (position.x + position.y) / 2;
        return isoVec;
    }

    private void Move() {
        moveInput = pInputActions.Player.Movement.ReadValue<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1);

        Vector2 moveVector = moveInput * moveSpeed * Time.fixedDeltaTime;
        if(isometricMovement)
            moveVector = IsometricConversion(moveVector);
        body.MovePosition(body.position + moveVector);
    }

    private void FixedUpdate() {
        Move();
    }
}
