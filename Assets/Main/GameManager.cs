using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake () { instance = this; }

    GameObject player;
    ThrowingController chef;

    public void SetPlayer(GameObject _player) { player = _player; }
    public GameObject GetPlayer() { return player; }

    public void SetChef(ThrowingController _chef) {chef = _chef; }
    public ThrowingController GetChef() { return chef; }
}
