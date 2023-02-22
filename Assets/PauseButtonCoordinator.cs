using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonCoordinator : MonoBehaviour
{
    public void TogglePause()
    {
        PauseManager.instance.TogglePauseMenu();
    }
}
