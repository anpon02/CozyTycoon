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

    private void Start()
    {
        if (KitchenManager.instance) KitchenManager.instance.SetChef(this);
    }
    public bool IsHoldingItem() {
        return heldItem != null;
    }

    public void HoldNewItem(ItemCoordinator item)
    {
        ResetHeldItem();
        item.Show();

        item.StopMoving();
        item.transform.parent = transform;
        item.transform.localPosition = Vector3.zero;
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

        heldItem.transform.parent = null;
        var dir = ((Vector3)mouseWorldPos - transform.position).normalized;
        var str = Mathf.Min(mouseDownTime, maxHoldTime)/ maxHoldTime;
        heldItem.GetRB().AddForce(dir * str * throwMult);
        AudioManager.instance.PlaySound(heldItem.GetItem().GetThrowSound(), heldItem.gameObject);

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

    public Item GetHeldItem()
    {
        return heldItem.GetItem();
    }
    
    public Item RemoveHeldItem()
    {
        var _item = heldItem.GetItem();
        Destroy(heldItem.gameObject);
        heldItem = null;
        return _item;
    }

}
