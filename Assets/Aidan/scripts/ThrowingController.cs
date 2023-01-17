using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingController : MonoBehaviour
{
    //click and hold to change throw, releasing will throw toward the mouse

    [SerializeField] Rigidbody2D heldRB;
    [SerializeField] float maxHoldTime;
    [SerializeField] float throwMult = 100;
    Vector2 mouseWorldPos;
    bool mouseDown;
    float mouseDownTime;

    public void HoldNewItem(GameObject itemObj)
    {
        var rb = itemObj.GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.LogError("ThrowingController was told to hold an item w/o a rigidbody"); return; }

        ResetHeldItem();
        rb.velocity = Vector2.zero;
        rb.transform.position = transform.position;
        heldRB = rb;
    }

    public void SetMousePos(InputAction.CallbackContext ctx)
    {
        var ScreenPos = ctx.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(ScreenPos);
    }

    public void ReadClickInput(InputAction.CallbackContext ctx)
    {
        if (heldRB == null) return;

        if (ctx.canceled && mouseDown) ReleaseThrow();
        if (ctx.started) mouseDown = true;
    }

    void ReleaseThrow()
    {
        if (heldRB == null) return;

        var dir = ((Vector3)mouseWorldPos - transform.position).normalized;
        var str = Mathf.Min(mouseDownTime, maxHoldTime)/ maxHoldTime;
        heldRB.AddForce(dir * str * throwMult);

        ResetHeldItem();
    }

    void ResetHeldItem()
    {
        mouseDown = false;
        mouseDownTime = 0;
        heldRB = null;
    }

    private void Update()
    {
        if (mouseDown) mouseDownTime += Time.deltaTime;
    }

}
