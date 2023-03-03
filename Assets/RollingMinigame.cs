using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;
        Complete();
    }
    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
