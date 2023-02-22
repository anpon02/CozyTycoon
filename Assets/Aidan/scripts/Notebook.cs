using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.notebook = this;
    }

    public void RecordInfo(string info, CharacterName character)
    {

    } 
}
