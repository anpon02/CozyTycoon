using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager instance;
    void Awake() { instance = this; }

    ThrowingController chef;

    [Header("Prefabs")]
    [SerializeField] GameObject itemCoordPrefab;

    //get + set
    public void SetChef(ThrowingController _chef) { chef = _chef; }
    public ThrowingController GetChef() { return chef; }


    //universal functions - try to find a better place for these??
    public ItemCoordinator CreateNewItemCoord(Item item, Vector3 pos)
    {
        var newGO = Instantiate(itemCoordPrefab, pos, Quaternion.identity);
        var coordScript = newGO.GetComponent<ItemCoordinator>();
        coordScript.SetItem(item);
        return coordScript;
    }
}
