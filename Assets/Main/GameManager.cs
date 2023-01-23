using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake () { instance = this; }

    GameObject player;
    OrderController orderController;
    CameraShake shakeScript;
    
    public void SetPlayer(GameObject _player) { player = _player; }
    public GameObject GetPlayer() { return player; }

    public void SetOrderController(OrderController _controller) { orderController = _controller; }
    public OrderController GetOrderController() { return orderController; }

    public void SetShakeScript(CameraShake _script) { shakeScript = _script; }
    public CameraShake getShakeScript() { return shakeScript; }

}
