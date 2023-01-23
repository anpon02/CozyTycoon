using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    // this will not be serialized later on
    [SerializeField] private bool isTaken;

    private void Awake() {
        isTaken = false;
    }

    public bool GetIsTaken() {
        return isTaken;
    }

    public void SetIsTaken(bool taken) {
        isTaken = taken;
    }
}
