using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake () { instance = this; }

    GameObject player;

    public void SetPlayer(GameObject _player)
    {
        print("got player");
        player = _player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
