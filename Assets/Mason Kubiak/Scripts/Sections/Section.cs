using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Section
{
    [SerializeField] private string sectionName;
    [SerializeField] private int sectionId;
    [SerializeField] private Transform sectionTransform;
    [SerializeField] private Vector3 cameraPosition;

    public Transform GetSectionTransform() {
        return sectionTransform;
    }

    public string GetSectionName() {
        return sectionName;
    }

    public int GetSectionId() {
        return sectionId;
    }

    public Vector3 GetCameraPosition() {
        return cameraPosition;
    }
}
