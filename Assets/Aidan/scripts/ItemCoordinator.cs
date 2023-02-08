using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    [SerializeField] Item item;

    ChefController chef;
    SpriteRenderer sRend;
    Vector3 targetPos;
    [SerializeField] float MinLerpDist = 0.25f;
    [SerializeField] float moveSmoothness = 0.025f;
    [HideInInspector] public bool travellingToChef;
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

    void EnableCollider()
    {
        var box = GetComponent<BoxCollider2D>();
        var poly = GetComponent<PolygonCollider2D>();
        if (box) box.enabled = true;
        if (poly) poly.enabled = true;
    }

    void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
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
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        if (SetChef()) chef.PickupItem(this);
    }
    
}
