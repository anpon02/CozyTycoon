using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int money;

    private void Start()
    {
        GameManager.instance.wallet = this;
    }
}
