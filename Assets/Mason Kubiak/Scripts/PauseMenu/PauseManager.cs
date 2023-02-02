using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("Canvases")]
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas settingsCanvas;

    [Header("Graphics Buttons")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private List<Transform> graphicsButtons;
    private int selectedGraphicsButton;

    [Header("Scenes")]
    [SerializeField] private List<string> sceneNames;

    private PauseInputActions pInputActions;
    private bool paused;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
        
        paused = false;

        // canvas setup
        pauseCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);

        // graphics setup
        selectedGraphicsButton = 2;

        // input actions setup
        pInputActions = new PauseInputActions();
        pInputActions.Pause.TogglePause.performed += PauseEvent;
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        pInputActions.Disable();
    }

    private void PauseEvent(InputAction.CallbackContext context) {
        TogglePauseMenu();
    }

    public void TogglePauseMenu() {
        // if on pause screen, exit back to game
        if(pauseCanvas.gameObject.activeInHierarchy) {
            pauseCanvas.gameObject.SetActive(false);
            settingsCanvas.gameObject.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }
        // open pause screen if not paused or in settings menu
        else {
            pauseCanvas.gameObject.SetActive(true);
            settingsCanvas.gameObject.SetActive(false);
            paused = true;
            Time.timeScale = 0;
            
        }
    }

    public void SelectGraphicsSetting(int buttonIndex) {
        // TODO: Get this function working
        // set selectedGraphicsButton to butotnIndex
        // loop through all buttons
        // if index == buttonIndex, make that button's color selected color
        // else, make that button's color unselected color
        // make each button's hover color selected color
    }

    public bool GetPaused() {
        return paused;
    }
}
