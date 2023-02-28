using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class ChefController : MonoBehaviour
{
    [SerializeField] ItemCoordinator heldItem;
    [SerializeField] Vector3 itemOffset;
    GameObject player;

    [Header("Sounds")]
    [SerializeField] int pickupSound;
    [SerializeField] int putdownSound;
    [SerializeField] ParticleSystem footStepParticles;
    
    [HideInInspector] public bool JustGotItem;


    public bool IsHoldingItem() {
        return heldItem != null;
    }

    public void PickupItem(ItemCoordinator iCoord)
    {
        JustGotItem = true;
        AudioManager.instance.PlaySound(pickupSound, gameObject);
        heldItem = iCoord;
    }

    public void ReadClickInput(InputAction.CallbackContext ctx)
    {
        if (heldItem == null) return;
        if (ctx.started) JustGotItem = false;
        if (ctx.canceled && JustGotItem == false) PlaceItem();
    }

    private void Start()
    {
        KitchenManager.instance.chef = this;
    }

    void PlaceItem()
    {
        var ws = KitchenManager.instance.hoveredController;
        if (ws == null || (ws.hasBigEquipment && heldItem.GetItem().isBigEquipment) || ws.minigameActive) return;
        AudioManager.instance.PlaySound(putdownSound, gameObject);

        ReleaseItem(KitchenManager.instance.hoveredController);
    }

    public void ReleaseItem(Vector3 pos)
    {
        heldItem.SetPosition(pos);
        heldItem = null;

    }

    public void ReleaseItem(WorkspaceController controller)
    {
        heldItem.SetPosition(controller.itemLerpTarget, _wsDest:controller);
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
        if (!player) player = GameManager.instance.player;
        if (!player) return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) footStepParticles.Play();
        else footStepParticles.Stop();

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
        if (heldItem == null) return null;

        var _item = heldItem.GetItem();
        Destroy(heldItem.gameObject);
        heldItem = null;
        return _item;
    }

}
