using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followVCam;
    [SerializeField] private CinemachineVirtualCamera mouseVCam;
    [SerializeField] Transform mouseCamTarget;
    [SerializeField] float exitDistance = 1.5f;
    [SerializeField] float maxDistFromPlayer = 2.5f;

    private Transform player;
    public bool followMouse;

    private void Awake() {
        followVCam.Priority = 2;
        mouseVCam.Priority = 1;
        mouseVCam.Follow = mouseCamTarget;
    }

    private void Start()
    {
        GameManager.instance.camScript = this;        
    }

    private void Update() {
        if (!SetPlayer()) return;
        else
            followVCam.Follow = player;

        //FollowMouse();
        SetTarget();
    }

    bool SetPlayer()
    {
        if (GameManager.instance == null) return false;
        player = GameManager.instance.player.transform;
        return player != null;
    }

    private void SetTarget() {
        if(!followMouse) return;

        // get mouse position in world, move mouseCamTarget to it, and restrict it to max distance from player
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseCamTarget.position = player.position + (worldMousePos - player.position).normalized * maxDistFromPlayer;

        // follow mouseCamTarget if mouse is beyond exit radius
        bool mouseInBounds = Vector2.Distance(worldMousePos, player.position) > exitDistance;
        followVCam.gameObject.SetActive(!mouseInBounds);
    }
}
