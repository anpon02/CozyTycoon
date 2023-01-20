using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class ThrowingController : MonoBehaviour
{
    

    [SerializeField] ItemCoordinator heldItem;
    [SerializeField] float maxHoldTime;
    [SerializeField] float throwMult = 100;
    Vector2 mouseWorldPos;
    bool mouseDown;
    float mouseDownTime;
    GameObject player;

    public bool IsHoldingItem() {
        return heldItem != null;
    }

    public void HoldNewItem(ItemCoordinator item)
    {
        ResetHeldItem();

        item.StopMoving();
        item.transform.position = transform.position;
        heldItem = item;
    }

    public void SetMousePos(InputAction.CallbackContext ctx)
    {
        var ScreenPos = ctx.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(ScreenPos);
    }

    public void ReadClickInput(InputAction.CallbackContext ctx)
    {
        if (heldItem == null) return;

        if (ctx.canceled && mouseDown) ReleaseThrow();
        if (ctx.started) mouseDown = true;
    }

    void ReleaseThrow()
    {
        if (heldItem == null) return;

        var dir = ((Vector3)mouseWorldPos - transform.position).normalized;
        var str = Mathf.Min(mouseDownTime, maxHoldTime)/ maxHoldTime;
        heldItem.GetRB().AddForce(dir * str * throwMult);

        ResetHeldItem();
    }

    void ResetHeldItem()
    {
        mouseDown = false;
        mouseDownTime = 0;
        heldItem = null;
    }

    private void Update()
    {
        SetPosition();
        if (mouseDown) mouseDownTime += Time.deltaTime;
    }

    void SetPosition()
    {
        if (!player) player = GameManager.instance.GetPlayer();
        if (!player) return;
        transform.position = player.transform.position;
    }

}
