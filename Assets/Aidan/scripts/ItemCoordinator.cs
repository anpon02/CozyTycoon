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
        KitchenManager.instance.ttCoord.DisplayItem(item);
        outline.gameObject.SetActive(true);
    }

    bool IsBigAndInWS() {
        return item.IsBigEquipment() && wsCoord != null && wsCoord.HeldItemCount > 1;
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
    }

    bool InReach()
    {
        return Vector2.Distance(KitchenManager.instance.GetChef().transform.position, transform.position) <= KitchenManager.instance.playerReach;
    }

    bool CanPickUp()
    {
        bool inreach = InReach();
        if (item.IsBigEquipment() && wsCoord != null) return (wsCoord.GetHeldItems().Count <= 1 && inreach);
        return inreach;
    }

    public void SetDisplayParent(Transform displayParent, WorkspaceCoordinator _wsCoord)
    {
        //print("setting display parent for: " + item.GetName() + ", " + _wsCoord.gameObject.name);
        wsCoord = _wsCoord;
        transform.parent = displayParent;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        HideQualityDisplay();
    }
    public bool InWS()
    {
        return wsCoord != null;
    }
    public void FreeFromDisplayParent()
    {
        //print(gameObject.name + " freed from parent: " + wsCoord.gameObject.name);
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

        var starIndex = -1;
        Color col = Color.black;
        foreach (var s in stars) s.gameObject.SetActive(false);
        return;

        if (item.GetQuality() == -1) return;

        var qual = item.GetQuality();
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
        }
    }

    bool SetChef()
    {
        if (KitchenManager.instance) chef = KitchenManager.instance.GetChef();
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
        item.SetQuality(1);
        GetReferences();
        if (item.IsBigEquipment()) {
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
