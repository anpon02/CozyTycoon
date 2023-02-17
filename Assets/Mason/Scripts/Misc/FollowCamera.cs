using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followVCam;
    [SerializeField] private CinemachineVirtualCamera stationaryVCam;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float maxDistFromPlayer = 2.5f;
    [SerializeField] Vector4 screenEdge = new Vector4(0.2f, 0.2f, 0.2f, 0.1f);
    private Transform player;
    public bool followMouse;


    private void Awake() {
        followVCam.Priority = 2;
        stationaryVCam.Priority = 1;
    }

    private void Start()
    {
        GameManager.instance.camScript = this;        
    }

    private void Update() {
        if (!SetPlayer()) return;

        FollowMouse();
    }

    void FollowMouse()
    {
        var screenPos = Input.mousePosition;
        screenPos.x /= Screen.width;
        screenPos.y /= Screen.height;
        bool mouseInBounds = screenPos.y < screenEdge.w || screenPos.y > 1 - screenEdge.y || screenPos.x < screenEdge.x || screenPos.x > 1 - screenEdge.z;
        mouseInBounds = mouseInBounds && followMouse;

        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var targetPos = Vector3.Lerp(cameraTarget.position, mouseInBounds ? worldMousePos : player.position, 0.2f);

        targetPos = LimitPosition(targetPos);
        
        cameraTarget.transform.position = targetPos;
    }

    Vector2 LimitPosition(Vector2 input)
    {
        input.x = Mathf.Clamp(input.x, player.position.x - maxDistFromPlayer, player.position.x + maxDistFromPlayer);
        input.y = Mathf.Clamp(input.y, player.position.y - maxDistFromPlayer, player.position.y + maxDistFromPlayer);
        return input;
    }

    bool SetPlayer()
    {
        var unset = player == null;
        if (GameManager.instance == null) return false;
        player = GameManager.instance.player.transform;
        if (unset && player != null) StartFollow();
        return player != null;
    }

    void StartFollow()
    {
        cameraTarget.transform.position = player.position;
        followVCam.Follow = cameraTarget;
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
