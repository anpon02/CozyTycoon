using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake () { instance = this; }

    GameObject player;
    OrderController orderController;
    CameraShake shakeScript;
    public UnityEvent OnStoreOpen;
    public UnityEvent OnStoreClose;
    [HideInInspector] public PlayerWallet wallet;
    
    public void SetPlayer(GameObject _player) { player = _player; }
    public GameObject GetPlayer() { return player; }

    public void SetOrderController(OrderController _controller) { orderController = _controller; }
    public OrderController GetOrderController() { return orderController; }

    public void SetShakeScript(CameraShake _script) { shakeScript = _script; }
    public CameraShake getShakeScript() { return shakeScript; }

}
