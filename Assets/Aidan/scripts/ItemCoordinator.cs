using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] List<SpriteRenderer> stars = new List<SpriteRenderer>();
    [SerializeField] Color topQuality;
    [SerializeField] Color medQuality;
    [SerializeField] Color lowQuality;
    [SerializeField] SpriteRenderer outline;

    ThrowingController chef;
    Rigidbody2D rb;
    SpriteRenderer sRend;
    public bool isFree;

    WorkspaceCoordinator wsCoord;
    Quaternion defaultRot;
    Vector3 defaultLocalScale;

    private void OnValidate() {
        if (item == null || string.IsNullOrEmpty(item.GetName())) return;

        if (PrefabStageUtility.GetCurrentPrefabStage() == null || PrefabStageUtility.GetCurrentPrefabStage() != PrefabStageUtility.GetPrefabStage(gameObject)) gameObject.name = item.GetName();
        if (sRend == null) GetReferences();
        sRend.sprite = item.GetSprite();
        outline.sprite = item.GetSprite();
        UpdateQualityDisplay();
    }

    private void Awake()
    {
        defaultLocalScale = transform.localEulerAngles;
        defaultRot = transform.rotation;
        outline.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if ((SetChef() && chef.GetHeldiCoord() == this) || !InReach() || IsBigAndInWS()) return;
        KitchenManager.instance.ttCoord.Display(item);
        outline.gameObject.SetActive(true);
    }

    bool IsBigAndInWS() {
        return item.isBigEquipment && wsCoord != null && wsCoord.HeldItemCount > 1;
    }

    private void OnMouseExit()
    {
        KitchenManager.instance.ttCoord.ClearText(item);
        outline.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (InReach()) sRend.color = Color.white;
        else sRend.color = new Color(1, 1, 1, 0.5f);
        outline.flipX = sRend.flipX;
    }

    bool InReach()
    {
        return Vector2.Distance(KitchenManager.instance.chef.transform.position, transform.position) <= KitchenManager.instance.playerReach;
    }

    bool CanPickUp()
    {
        bool inreach = InReach();
        if (item.isBigEquipment && wsCoord != null) return (wsCoord.GetHeldItems().Count <= 1 && inreach);
        return inreach;
    }

    public void SetDisplayParent(Transform displayParent, WorkspaceCoordinator _wsCoord)
    {
        if (displayParent.GetComponent<SpriteRenderer>().flipX) sRend.flipX = true;
        if (!item.isBigEquipment) outline.sortingOrder = 0;
        wsCoord = _wsCoord;
        transform.parent = displayParent;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * item.scaleMult;

        HideQualityDisplay();
    }
    public bool InWS()
    {
        return wsCoord != null;
    }
    public void FreeFromDisplayParent()
    {
        if (!item.isBigEquipment) outline.sortingOrder = -1;
        sRend.flipX = false;
        wsCoord = null;
        transform.parent = null;
        transform.localEulerAngles = defaultLocalScale;
        transform.rotation = defaultRot;

        UpdateQualityDisplay();
    }

    void HideQualityDisplay() {
        foreach (var s in stars) s.gameObject.SetActive(false);
    }

    void UpdateQualityDisplay()
    {
        if (item == null) return;
        foreach (var s in stars) s.gameObject.SetActive(false);

        /*
        Color col = Color.black;
        var starIndex = -1;
        if (item.quality == -1) return;

        var qual = item.quality;
        if (qual < 0.3f) {
            col = lowQuality;
            starIndex = 1;
        }
        else if (qual > 0.9f) {
            col = topQuality;
            starIndex = 3;
        }
        else {
            col = medQuality;
            starIndex = 2;
        } 

        for (int i = 0; i < starIndex; i++) {
            stars[i].gameObject.SetActive(true);
            stars[i].color = col;
        }*/
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
        item = Instantiate(item);
        GetReferences();
        if (item.isBigEquipment) {
            sRend.sortingOrder = -1;
            outline.sortingOrder = -2;
        }
    }

    void GetReferences() {
        sRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (!SetChef() || !CanPickUp() || chef.GetHeldItem() != null) return;
        if (wsCoord) {
            wsCoord.removeItem(this);
            FreeFromDisplayParent();
        }
        chef.HoldNewItem(this);
    }

    public Rigidbody2D GetRB()
    {
        return rb;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public Sprite GetItemSprite()
    {
        return item.GetSprite();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateQualityDisplay();
    }
}
