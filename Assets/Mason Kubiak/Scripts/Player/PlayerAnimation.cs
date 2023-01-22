using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerInputActions pInputActions;
    private Animator anim;

    private string[] stillAnims = {"Still_N", "Still_NW", "Still_W", "Still_SW", 
                                   "Still_S", "Still_SE", "Still_E", "Still_NE"};
    private string[] moveAnims = {"Move_N", "Move_NW", "Move_W", "Move_SW",
                                  "Move_S", "Move_SE", "Move_E", "Move_NE"};
    private Vector2 input;
    private int lastDir = 4;

    private void Awake() {
        pInputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void SetLastDir(Vector2 direction) {
        // normalize angle to int between 0 and 7
        float angle = Vector2.SignedAngle(Vector2.up, direction.normalized) + 22.5f;
        angle = angle < 0 ? angle + 360 : angle;
        lastDir = Mathf.FloorToInt(angle / 45);
    }

    private void SetDirection(Vector2 direction) {
        
        string[] animStates = null;

        // if not moving
        if(direction.magnitude < 0.1f) {
            animStates = stillAnims;
        }
        // if moving
        else {
            animStates = moveAnims;
            SetLastDir(direction);
        }

        anim.SetTrigger("Player_" + animStates[lastDir]);
    }

    public int GetLastDir() {
        return lastDir;
    }

    private void Update() {
        input = pInputActions.Player.Movement.ReadValue<Vector2>();
        SetDirection(input);
    }
}
