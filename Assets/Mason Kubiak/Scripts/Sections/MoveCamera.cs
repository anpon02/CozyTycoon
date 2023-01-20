using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float interpolationValue;
    private Camera cam;
    private Vector3 currentPos;
    private int currentRoomID;

    private void Awake() {
        cam = GetComponent<Camera>();
        currentRoomID = 0;
    }

    private void Start() {
        if(SectionManager.instance == null)
            this.enabled = false;
        else
            currentPos = SectionManager.instance.GetSections()[0].GetCameraPosition();
    }

    private void Update() {
        if(cam.transform.position == currentPos) return;

        if(Vector3.Distance(cam.transform.position, currentPos) > 0.05f)
            cam.transform.position = Vector3.Lerp(cam.transform.position, currentPos, interpolationValue);
        else
            cam.transform.position = currentPos;
    }

    public Camera GetCam() {
        return cam;
    }

    public void SetNewRoom(int roomID) {
        currentPos = SectionManager.instance.GetSections()[roomID].GetCameraPosition();
        currentRoomID = roomID;
    }
}
