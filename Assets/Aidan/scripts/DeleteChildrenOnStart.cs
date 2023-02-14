using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteChildrenOnStart : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
