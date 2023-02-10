using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("Canvases")]
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas settingsCanvas;

    [Header("Graphics Buttons")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private List<Button> graphicsButtons;
    private int selectedGraphicsButton;

    [Header("Volume Settings")]
    [SerializeField] private Slider volSlider;

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
        SelectGraphicsSetting(selectedGraphicsButton);

        // input actions setup
        pInputActions = new PauseInputActions();
        pInputActions.Pause.TogglePause.performed += PauseEvent;
    }

    private void Start() {
        // volume setup
        volSlider.value = 0.5f;
        ChangeVolume();
    }

    private void OnEnable() {
        pInputActions.Enable();
    }

    private void OnDisable() {
        if (pInputActions != null) pInputActions.Disable();
    }

    private void PauseEvent(InputAction.CallbackContext context) {
        TogglePauseMenu();
    }

    private void SetPixelation() {
        // TODO:
        // MAKE A POST PROCESSING EFFECT THAT PIXELIZES SCREEN
        // SET VALUE OF PIXELIZATION
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
        // set selectedGraphicsButton to butotnIndex
        selectedGraphicsButton = buttonIndex;

        // loop through all buttons
        for(int i = 0; i < graphicsButtons.Count; ++i) {
            ColorBlock buttonColors = graphicsButtons[i].colors;
            buttonColors.highlightedColor = selectedColor;

            // if index == buttonIndex, make that button's color selected color
            if(i == buttonIndex) {
                buttonColors.normalColor = selectedColor;
                SetPixelation();  // can probably set pixelization value using some math with the index
            }
            else {
                buttonColors.normalColor = unselectedColor;
                SetPixelation();  // can probably set pixelization value using some math with the index
            }
            graphicsButtons[i].colors = buttonColors;
        }
    }

    public void ChangeVolume() {
        AudioManager.instance.masterVolume = volSlider.value * 2.0f;
    }

    public bool GetPaused() {
        return paused;
    }
}
