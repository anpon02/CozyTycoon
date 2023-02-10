using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        foreach (var g in gameObjects) g.SetActive(true);
    }
}
