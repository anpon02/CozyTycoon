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
    public int ongoingMinigames;

    [Header("Contents")]
    [SerializeField] GameObject contentsParent;
    [SerializeField] TextMeshProUGUI content1;
    [SerializeField] TextMeshProUGUI content2;
    [SerializeField] TextMeshProUGUI content3;
    [SerializeField] TextMeshProUGUI content4;
    [SerializeField] TextMeshProUGUI content5;

    [Header("Recipe Options")]
    [SerializeField] List<TextMeshProUGUI> recipeOptionTexts = new List<TextMeshProUGUI>();
    List<string> recipeOptionNames = new List<string>();

    [Header("ProgressBar")]
    [SerializeField] GameObject progressBarParent;
    public Slider progressSlider;

    [Header("Sounds")]
    public int progressSound;
    [SerializeField] int completeSound, failSound;

    public GameObject bigEquipmentSprite;
    public GameObject dispalyItemSprite;
    

    public void SelectRecipeOption(int num)
    {
        int index = num - 1;

        ws.chosenRecipe = recipeOptionNames[index];
        if (!string.IsNullOrEmpty(ws.chosenRecipe)) ws.StartCooking();
        HideRecipeOptions();
    }

    public void ShowRecipeOptions(List<Item> options)
    {
        if (IsMinigameActive()) return;

        for (int i = 0; i < options.Count; i++) {
            SetupROButton(recipeOptionTexts[i], options[i].GetName(), i);
        }
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        for (int i = 0; i < 6; i++) {
            recipeOptionNames.Add("");
        }
    }

    void SetupROButton(TextMeshProUGUI text, string _name, int num)
    {
        text.text = _name;
        recipeOptionNames[num] = _name;
        text.transform.parent.gameObject.SetActive(true);
    }

    public void HideRecipeOptions()
    {
        ws.chosenRecipe = "";
        foreach (var t in recipeOptionTexts) { t.text = ""; t.transform.parent.gameObject.SetActive(false); }
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
