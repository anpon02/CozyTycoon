using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager instance;
    void Awake() { instance = this; }

    ThrowingController chef;
    [Range(0, 1)]
    [SerializeField] float taskFactor = 0.5f;
    public float playerReach = 5;
    [SerializeField] Vector2 _midHighQualityCutoff = new Vector2(0.3f, 0.9f);
    public Vector2 midHighQualityCutoff { get { return _midHighQualityCutoff; } }
    [HideInInspector] public ToolipCoordinator ttCoord;

    [Header("Prefabs")]
    [SerializeField] GameObject itemCoordPrefab;

    public void SetChef(ThrowingController _chef) { chef = _chef; }
    public ThrowingController GetChef() { return chef; }

    public float GetTaskFactor()
    {
        return taskFactor;
    }


    //universal functions - try to find a better place for these??
    public ItemCoordinator CreateNewItemCoord(Item item, Vector3 pos , float quality)
    {
        item.SetQuality(quality);
        var newGO = Instantiate(itemCoordPrefab, pos, Quaternion.identity);
        var coordScript = newGO.GetComponent<ItemCoordinator>();
        coordScript.SetItem(item);
        return coordScript;
    }
    public ItemCoordinator CreateNewItemCoord(Item item, Vector3 pos)
    {
        return CreateNewItemCoord(item, pos, item.GetQuality());
    }
}
