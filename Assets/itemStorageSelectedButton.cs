using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class itemStorageSelectedButton : MonoBehaviour
{
    [SerializeField] itemGridCoordinator gridCoord;
    [HideInInspector] public int itemIndex;

    public void StartHover() {
        gridCoord.Selected = itemIndex;
    }
    public void EndHover() {
        gridCoord.Selected = -1;
    }
}
