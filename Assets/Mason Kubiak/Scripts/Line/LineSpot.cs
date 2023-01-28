using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineSpot
{
    [SerializeField] private Vector3 placeCoordinates;
    [SerializeField] private bool placeIsTaken = false;

    public Vector3 GetPlaceCoordinates() {
        return placeCoordinates;
    }

    public bool GetPlaceIsTaken() {
        return placeIsTaken;
    }

    public void SetPlaceIsTaken(bool isTaken) {
        placeIsTaken = isTaken;
    }
}
