using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMinigame : MonoBehaviour
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
