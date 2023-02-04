using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowCatalogCoordinator : MonoBehaviour
{
    bool hidden;
    [SerializeField] Animator animator;
    public void HideShow()
    {
        hidden = !hidden;
        animator.SetTrigger(hidden ? "show" : "hide");
    }
}
