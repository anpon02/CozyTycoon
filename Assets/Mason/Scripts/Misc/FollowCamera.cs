using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followVCam;
    [SerializeField] private CinemachineVirtualCamera stationaryVCam;
    private Transform player;
    private PolygonCollider2D cameraCollider;

    private void Awake() {
        cameraCollider = GetComponent<PolygonCollider2D>();
        followVCam.Priority = 2;
        stationaryVCam.Priority = 1;
    }

    private void Update() {
        if(player || GameManager.instance.GetPlayer() == null)
            return;
        else {
            player = GameManager.instance.GetPlayer().transform;
            followVCam.Follow = player;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // follow player
        if(other.tag == "Player") {
            followVCam.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // stationary view
        if(other.tag == "Player") {
            stationaryVCam.transform.position = new Vector3(player.position.x, player.position.y, stationaryVCam.transform.position.z);
            followVCam.enabled = false;
        }
    }
}
