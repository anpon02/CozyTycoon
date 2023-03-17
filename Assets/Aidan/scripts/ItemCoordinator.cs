using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    public Item item, side;
    [SerializeField] float MinLerpDist = 0.25f;
    [SerializeField] float moveSmoothness = 0.025f;
    public bool travellingToChef;
    [SerializeField] GameObject plate, sidePlate;
    public bool plated;
    [SerializeField] ParticleSystem particles;
    [SerializeField] SpriteRenderer sideSRend;

    [HideInInspector] public ItemStorage home;
    ChefController chef;
    SpriteRenderer sRend;
    Vector3 targetPos;
    Rigidbody2D rb;
    [HideInInspector] public WorkspaceController wsDest;
    [HideInInspector] public bool showPlate, showSidePlate;

    public Sprite GetItemSprite()
    {
        return item.GetSprite();
    }

    private void OnValidate()
    {
        UpdateDisplay();
    }

    public void SetPosition(Vector3 newPos, bool toChef = false, WorkspaceController _wsDest = null)
    {
        wsDest = _wsDest;
        travellingToChef = toChef;
        targetPos = newPos;
    }

    void UpdateDisplay()
    {
        if (KitchenManager.instance) {
            showPlate = KitchenManager.instance.needPlate(item);
            if (!showPlate) plated = true;
            showSidePlate = KitchenManager.instance.needPlate(side);
        }

        if (sRend == null) GetReferences();

        sRend.sprite = item == null ? null : item.GetSprite();
        plate.SetActive(plated && showPlate);
        sidePlate.SetActive(showSidePlate);
      
        if (side == null) { sideSRend.gameObject.SetActive(false); return; }
        sideSRend.sprite = side.GetSprite();
        sideSRend.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (Application.isPlaying && item != null) gameObject.name = item.GetName();
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
        UpdateDisplay();
    }

    void MoveToTargetPos()
    {
        if (Vector3.Distance(targetPos, transform.position) <= MinLerpDist) {
            travellingToChef = false;
            transform.position = targetPos;

            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
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

    public bool CanAccept(Item _item)
    {
        if (!plated || !_item.menuItem || !item.menuItem) return false;
        if (_item.side != item.side && (item == null || side == null)) return true;
        return false;
    }

    public void AddItem(Item _item)
    {
        if (!CanAccept(_item)) return;

        if (item.side) {
            side = item;
            item = _item;
        }
        else side = _item;
    }

    public void SetItem(Item _item) {
        item = _item;
        UpdateDisplay();
    }

    public Item GetItem() {
        return item;
    }

    private void Start()
    {
        targetPos = transform.position;
        item = Instantiate(item);
        GetReferences();
        particles.Play();
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
