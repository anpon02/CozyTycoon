using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoordinator : MonoBehaviour
{
    [SerializeField] Item item;
    ThrowingController chef;
    

    Rigidbody2D rb;
    SpriteRenderer sRend;

    private void OnValidate() {
        if (item == null || string.IsNullOrEmpty(item.GetName())) return;
        
        gameObject.name = item.GetName();
        if (sRend == null) GetReferences();
        sRend.sprite = item.GetSprite();
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
