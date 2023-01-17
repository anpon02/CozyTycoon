using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ThrowingController chef;
    [SerializeField] Sprite sprite;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        chef.HoldNewItem(gameObject);
    }

    public Sprite GetItemSprite()
    {
        return sprite;
    }

    public void DisablePhysics()
    {
        rb.Sleep();
    }

    public void EnablePhysics()
    {
        rb.WakeUp();
    }
}
