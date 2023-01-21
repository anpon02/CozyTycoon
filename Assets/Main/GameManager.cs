using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake () { instance = this; }

    GameObject player;
    OrderController orderController;
    
    public void SetPlayer(GameObject _player) { player = _player; }
    public GameObject GetPlayer() { return player; }

    public void SetOrderController(OrderController _controller) { orderController = _controller; }
    public OrderController GetOrderController() { return orderController; }


}
