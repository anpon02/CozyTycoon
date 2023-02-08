using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChefController : MonoBehaviour
{
    [SerializeField] ItemCoordinator heldItem;
    [SerializeField] Vector3 itemOffset;
    GameObject player;

    [Header("Sounds")]
    [SerializeField] int pickupSound;

    public bool IsHoldingItem() {
        return heldItem != null;
    }

    public void PickupItem(ItemCoordinator iCoord)
    {
        AudioManager.instance.PlaySound(pickupSound, gameObject);
        heldItem = iCoord;
    }

    public void ReadClickInput(InputAction.CallbackContext ctx)
    {
        if (heldItem == null) return;

        if (ctx.started) PlaceItem(); 
    }

    private void Start()
    {
        KitchenManager.instance.chef = this;
    }

    void PlaceItem()
    {
        var ws = KitchenManager.instance.hoveredController;
        if (ws == null) return;

        heldItem.SetPosition(KitchenManager.instance.hoveredController.itemLerpTarget);
        heldItem = null;
    }

    private void Update()
    {
        SetPosition();
        itemOffset.x = Mathf.Abs(itemOffset.x) * Mathf.Abs(player.transform.localEulerAngles.y) > 0.1f ? -1 : 1;
        if (heldItem) SetHeldItemPos();
    }

    void SetHeldItemPos()
    {
        heldItem.SetPosition(transform.position + itemOffset, true);
    }

    void SetPosition()
    {
        if (!player) player = GameManager.instance.GetPlayer();
        if (!player) return;
        transform.position = player.transform.position;
    }

    public Item GetHeldItem()
    {
        if (heldItem == null) return null;
        return heldItem.GetItem();
    }

    public ItemCoordinator GetHeldiCoord()
    {
        return heldItem;
    }
    
    public Item RemoveHeldItem()
    {
        var _item = heldItem.GetItem();
        Destroy(heldItem.gameObject);
        heldItem = null;
        return _item;
    }

}
