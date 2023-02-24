using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteAlways]
public class WorkstationUICoordinator : MonoBehaviour
{
    [SerializeField] WorkspaceController ws;

    [Header("Minigames")]
    [SerializeField] KnifeMinigame knifeMG;
    [SerializeField] PanMinigame panMG;
    [SerializeField] MixerMinigame mixerMG;
    [SerializeField] PotMinigame potMG;
    [SerializeField] BakingMinigame bakingMG;
    [SerializeField] RollingMinigame rollingMG;
    [SerializeField] CoffeeMinigame coffeeMG;
    [SerializeField] GraterMinigame graterMG;
    [HideInInspector] public int ongoingMinigames;

    [Header("Contents")]
    [SerializeField] GameObject contentsParent;
    [SerializeField] TextMeshProUGUI content1;
    [SerializeField] TextMeshProUGUI content2;
    [SerializeField] TextMeshProUGUI content3;
    [SerializeField] TextMeshProUGUI content4;
    [SerializeField] TextMeshProUGUI content5;

    [Header("Recipe Options")]
    [SerializeField] TextMeshProUGUI ro1;
    [SerializeField] TextMeshProUGUI ro2, ro3;
    string roName1, roName2, roName3;

    [Header("ProgressBar")]
    [SerializeField] GameObject progressBarParent;
    public Slider progressSlider;

    [Header("Sounds")]
    public int progressSound;
    [SerializeField] int completeSound, failSound;
    
    public void SelectRecipeOption(int num)
    {
        if (num == 1) ws.chosenRecipe = roName1;
        if (num == 2) ws.chosenRecipe = roName2;
        if (num == 3) ws.chosenRecipe = roName3;
        if (!string.IsNullOrEmpty(ws.chosenRecipe)) ws.StartCooking();
        HideRecipeOptions();
    }

    public void ShowRecipeOptions(List<Item> options)
    {
        if (IsMinigameActive()) return;

        if (options.Count > 0) SetupROButton(ro1, options[0].GetName(), 0);
        if (options.Count > 1) SetupROButton(ro2, options[1].GetName(), 1);
        if (options.Count > 2) SetupROButton(ro3, options[2].GetName(), 2);
    }

    void SetupROButton(TextMeshProUGUI text, string _name, int num)
    {
        text.text = _name;
        if (num == 0) roName1 = _name;
        if (num == 1) roName2 = _name;
        if (num == 2) roName3 = _name;
        text.transform.parent.gameObject.SetActive(true);
    }

    public void HideRecipeOptions()
    {
        ws.chosenRecipe = "";
        ro1.transform.parent.gameObject.SetActive(false);
        ro2.transform.parent.gameObject.SetActive(false);
        ro3.transform.parent.gameObject.SetActive(false);
        roName1 = roName2 = roName3 = "";
    }

    public void StartMinigame(Minigame minigame)
    {
        KitchenManager.instance.minigameStarted = true;
        GameManager.instance.camScript.followMouse = false;

        if (minigame == Minigame.KNIFE) knifeMG.gameObject.SetActive(true);
        if (minigame == Minigame.PAN) panMG.gameObject.SetActive(true);
        if (minigame == Minigame.MIXER) mixerMG.gameObject.SetActive(true);
        if (minigame == Minigame.POT) potMG.gameObject.SetActive(true);
        if (minigame == Minigame.ROLLING_PIN) rollingMG.gameObject.SetActive(true);
        if (minigame == Minigame.COFFEE_MAKER) coffeeMG.gameObject.SetActive(true);
        if (minigame == Minigame.GRATER) graterMG.gameObject.SetActive(true);
        if (minigame == Minigame.BAKING_TRAY) bakingMG.gameObject.SetActive(true);

        if (minigame == Minigame.NONE) CompleteRecipe();
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        progressBarParent.SetActive(false);

        if (!Application.isPlaying) return;

        DisplayContents();
        DisplayProgressSlider();
    }

    public void PlayFailSound()
    {
        AudioManager.instance.PlaySound(failSound, gameObject);
    }

    void DisplayProgressSlider()
    {
        progressBarParent.SetActive(IsMinigameActive());
    }

    public bool IsMinigameActive()
    {
        return ongoingMinigames > 0;
    }
    public void CompleteRecipe()
    {
        AudioManager.instance.PlaySound(completeSound, gameObject);
        progressSlider.value = 0;
        ws.CompleteRecipe();
        KitchenManager.instance.minigameCompleted = true;
        GameManager.instance.camScript.followMouse = true;
    }

    public void AddProgress(float amount)
    {
        progressSlider.value += amount;
    }  

    void DisplayContents()
    {
        contentsParent.SetActive(!IsMinigameActive() && ws == KitchenManager.instance.hoveredController);
        content2.text = content3.text = content4.text = content5.text = "";

        content1.text = ws.workSpaceType.ToString();
        var list = ws.GetItemList();
        if (list.Count > 0) content2.text = list[0].GetName().ToString();
        if (list.Count > 1) content3.text = list[1].GetName().ToString();
        if (list.Count > 2) content4.text = list[2].GetName().ToString();
        if (list.Count > 3) content5.text = list[3].GetName().ToString();
    }
}
