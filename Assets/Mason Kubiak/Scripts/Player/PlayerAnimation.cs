using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerInputActions pInputActions;
    private Animator anim;
    private SpriteRenderer sprRenderer;
    private Vector2 input;

    private void Awake() {
        pInputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void Update() {
        if(PauseManager.instance && !PauseManager.instance.GetPaused()) {
            input = pInputActions.Player.Movement.ReadValue<Vector2>();

            if(input.x < 0)
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            else if(input.x > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            
            if(input.magnitude > 0.1f)
                anim.SetTrigger("ChefRun");
            else if(input.magnitude < 0.1f)
                anim.SetTrigger("ChefIdle");
        }
    }
}
