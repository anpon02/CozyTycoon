using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement movement;
    private Animator anim;
    private Vector2 input;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.paused) return;
        
        input = movement.moveInput;

        if(input.x < 0 || (movement.isometricMovement && input.y > 0))
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        else if(input.x > 0 || (movement.isometricMovement && input.y < 0))
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            
        if(input.magnitude > 0.1f)
            anim.SetTrigger("ChefRun");
        else if(input.magnitude < 0.1f)
            anim.SetTrigger("ChefIdle");
    }
}
