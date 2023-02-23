using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] GameObject mainParent, settingsParent, helpParent, greyOut;

    [Header("Settings")]
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider musicVolSlider, sfxVolSlider;
    [SerializeField] Image assistUnchecked;
    [SerializeField] GameObject assistChecked;

    [Header("Settings")]
    [SerializeField] int menusclickSound;

    private PauseInputActions pInputActions;
    [HideInInspector] public bool paused;

    public void PlaySound() {
        AudioManager.instance.PlaySound(menusclickSound, gameObject);
    }

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
        
        settingsParent.SetActive(false);
        mainParent.SetActive(false);
        helpParent.SetActive(false);
        greyOut.SetActive(false);

        pInputActions = new PauseInputActions();
        pInputActions.Pause.TogglePause.performed += PauseEvent;
    }

    private void Start() {
        masterVolSlider.value = 1f;
        musicVolSlider.value = 1f;
        sfxVolSlider.value = 1f;
        ChangeMasterVol();
        ChangeMusicVol();
        ChangeSFXVol();
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

    public void TogglePauseMenu() {
        if(mainParent.activeInHierarchy) UnPauseGame();
        else PauseGame();
    }

    void UnPauseGame()
    {
        mainParent.SetActive(false);
        settingsParent.SetActive(false);
        paused = false;
        greyOut.SetActive(false);
        Time.timeScale = 1;
    }

    void PauseGame()
    {
        mainParent.SetActive(true);
        settingsParent.SetActive(false);
        paused = true;
        greyOut.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ChangeMasterVol() {
        AudioManager.instance.masterVolume = masterVolSlider.value;
    }

    public void ChangeSFXVol()
    {
        
    }

    public void ChangeMusicVol()
    {

    }

    public void ToggleAssistMode()
    {
        if (!GameManager.instance.assistMode) EnableAssistMode();
        else DisableAssistMode();
    }

    void DisableAssistMode()
    {
        GameManager.instance.assistMode = false;
        assistChecked.SetActive(false);
        assistUnchecked.color = new Color(1, 1, 1, 0.2f);
    }

    void EnableAssistMode()
    {
        GameManager.instance.assistMode = true;
        assistChecked.SetActive(true);
        assistUnchecked.color = new Color(1, 1, 1, 0.5f);
    }
}
