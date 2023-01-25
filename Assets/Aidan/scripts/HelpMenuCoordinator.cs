using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuCoordinator : MonoBehaviour
{
    [SerializeField] GameObject mainParent;

    public void Toggle()
    {
        mainParent.SetActive(!mainParent.activeInHierarchy);
    }
}
