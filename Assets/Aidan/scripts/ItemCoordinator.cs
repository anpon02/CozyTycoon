using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    public Item item;
    [SerializeField] float MinLerpDist = 0.25f;
    [SerializeField] float moveSmoothness = 0.025f;
    public bool travellingToChef;
    [SerializeField] GameObject plate;
    public bool plated;

    [HideInInspector] public ItemStorage home;
    ChefController chef;
    SpriteRenderer sRend;
    Vector3 targetPos;
    Rigidbody2D rb;

    public Sprite GetItemSprite()
    {
        return item.GetSprite();
    }

    private void OnValidate()
    {
        if (item == null || string.IsNullOrEmpty(item.GetName())) return;

        if (sRend == null) GetReferences();
        sRend.sprite = item.GetSprite();
    }

    public void SetPosition(Vector3 newPos, bool toChef = false)
    {
        travellingToChef = toChef;
        targetPos = newPos;
    }

    private void Awake()
    {
        if (Application.isPlaying) gameObject.name = item.GetName();
    }

    private void OnMouseEnter()
    {
        if ((SetChef() && chef.GetHeldiCoord() == this) || !InReach()) return;
        KitchenManager.instance.ttCoord.Display(item);
    }

    private void OnMouseExit()
    {
        KitchenManager.instance.ttCoord.ClearText(item);
    }

    private void Update()
    {
        plate.SetActive(plated);
        if (InReach()) sRend.color = Color.white;
        else sRend.color = new Color(1, 1, 1, 0.5f);
        MoveToTargetPos();
    }

    void MoveToTargetPos()
    {
        if (Vector3.Distance(targetPos, transform.position) <= MinLerpDist) {
            travellingToChef = false;
            transform.position = targetPos;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.025f);
    }

    private void OnDestroy()
    {
        if (home) home.InstanceDestructionCallback(this);
    }

    bool InReach()
    {
        return Vector2.Distance(KitchenManager.instance.chef.transform.position, transform.position) <= KitchenManager.instance.playerReach;
    }

    bool SetChef()
    {
        if (KitchenManager.instance) chef = KitchenManager.instance.chef;
        return chef != null;
    }

    public void SetItem(Item _item) {
        item = _item;
        OnValidate();
    }
    public Item GetItem() {
        return item;
    }

    private void Start()
    {
        targetPos = transform.position;
        item = Instantiate(item);
        GetReferences();
    }

    void GetReferences() {
        sRend = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (chef.IsHoldingItem()) return;

        if (!GetComponent<Rigidbody2D>()) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }

        if (SetChef()) chef.PickupItem(this);
    }
    
}
