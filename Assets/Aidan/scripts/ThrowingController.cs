using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingController : MonoBehaviour
{
    //click and hold to change throw, releasing will throw toward the mouse

    [SerializeField] GameObject heldObj;
    Vector2 mouseWorldPos;
    bool mouseDown;
    float mouseDownTime;

    public void SetMousePos(InputAction.CallbackContext ctx)
    {
        var ScreenPos = ctx.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(ScreenPos);
    }

    public void ReadClickInput(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled) { ReleaseThrow(); mouseDown = false; }
        if (ctx.started) mouseDown = true;
    }

    void ReleaseThrow()
    {
        print("time: " + mouseDownTime);
    }

    private void Update()
    {
        if (mouseDown) mouseDownTime += Time.deltaTime;
    }

}
