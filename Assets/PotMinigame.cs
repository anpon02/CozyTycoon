using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    private void OnEnable()
    {
        Complete();
    }
    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
