using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonCoordinator : MonoBehaviour
{
    public void TogglePause()
    {
        AudioManager.instance.PlaySound(27, gameObject);
        PauseManager.instance.TogglePauseMenu();
    }
}
