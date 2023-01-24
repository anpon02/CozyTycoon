using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] List<SpriteRenderer> stars = new List<SpriteRenderer>();
    [SerializeField] Color topQuality;
    [SerializeField] Color medQuality;
    [SerializeField] Color lowQuality;

    ThrowingController chef;
    Rigidbody2D rb;
    SpriteRenderer sRend;

    private void OnValidate() {
        if (item == null || string.IsNullOrEmpty(item.GetName())) return;

        if (PrefabStageUtility.GetCurrentPrefabStage() == null || PrefabStageUtility.GetCurrentPrefabStage() != PrefabStageUtility.GetPrefabStage(gameObject)) gameObject.name = item.GetName();
        if (sRend == null) GetReferences();
        sRend.sprite = item.GetSprite();
        UpdateRating();
    }

    void UpdateRating()
    {
        if (item == null) return;

        var starIndex = -1;
        Color col = Color.black;
        foreach (var s in stars) s.gameObject.SetActive(false);
        if (item.GetQuality() == -1) return;

        switch (item.GetQuality()) {
            case < 0.3f:
                col = lowQuality;
                starIndex = 1;
                break;
            case > 0.9f:
                col = topQuality;
                starIndex = 3;
                break;
            default:
                col = medQuality;
                starIndex = 2;
                break;
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
    }

    void GetReferences() {
        sRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (!SetChef()) return;
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
    }
}
