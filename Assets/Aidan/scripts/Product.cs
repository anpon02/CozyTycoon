using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newProduct", menuName = "ScriptableObjects/Product", order = 1)]
public class Product : ScriptableObject
{
    public Sprite imgSprite;
    public string productName, description;
    public int price;
    [TextArea(4, 10)]
    public string unlocks;
}
