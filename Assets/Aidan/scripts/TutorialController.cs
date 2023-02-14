using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] GameObject shop;
    [SerializeField] Item mixer;
    [SerializeField] float offset;

    [SerializeField] List<string> tutorialText = new List<string>();
    int tutorialIndex;

    GameManager gMan;
    bool waiting;
    
    bool waitingForWASD;
    bool waitingToTakeOrder;
    bool waitingToStartDialogue;
    bool waitingToSelectRecipe;
    bool waitingToDeliverFood;
    bool waitingToOpenShop;
    bool waitingToBuyMixer;

    string WASDstring = "";
    bool storeOpen = false;

    private void Start()
    {
        gMan = GameManager.instance;
        DisplayLine();
    }
    public void DisplayLine()
    {
        if (tutorialIndex == tutorialText.Count) gameObject.SetActive(false);
        if (waiting || tutorialIndex >= tutorialText.Count) return;

        promptText.text = tutorialText[tutorialIndex];
        ParseLine(tutorialText[tutorialIndex]);
        tutorialIndex += 1;
    }

    void ParseLine(string line)
    {
        waitingForWASD = line.Contains("WASD");
        if (line.Contains("open!")) OpenStore();
        waitingToTakeOrder = line.Contains("take their order");
        waitingToStartDialogue = line.Contains("TALK TO EVERYONE");
        waitingToSelectRecipe = line.Contains("select the recipe");
        waitingToDeliverFood = line.Contains("deliver the meal");
        waitingToOpenShop = line.Contains("shop button");
        waitingToBuyMixer = line.Contains("buying a mixer");
    }

    void OpenStore()
    {
        storeOpen = true;
        gMan.timeScript.UnpauseTime();
        gMan.timeScript.time = 0.15f;
    }

    void Update()
    {
        waiting = waitingForWASD || waitingToTakeOrder || waitingToStartDialogue || 
            waitingToSelectRecipe || waitingToDeliverFood || waitingToOpenShop || waitingToBuyMixer;

        if (waitingForWASD) CheckWASD();
        if (waitingToTakeOrder) CheckTakeOrder();
        if (waitingToStartDialogue) CheckDialogueStart();
        if (waitingToSelectRecipe) CheckForSelectRecipe();
        if (waitingToDeliverFood) CheckForDeliverFood();
        if (waitingToOpenShop) CheckForOpenShop();
        if (waitingToBuyMixer) CheckForBoughtMixer();

        if (!storeOpen && gMan.timeScript) gMan.timeScript.PauseTime();
    }

    void CheckForBoughtMixer()
    {
        waitingToBuyMixer = !mixer.IsPresentInList(KitchenManager.instance.unlockedEquipment);
        if (waitingToBuyMixer) return;

        waiting = false;
        DisplayLine();
    }

    void CheckForOpenShop()
    {
        print("AHH");
        waitingToOpenShop = !shop.activeInHierarchy;
        if (waitingToOpenShop) return;

        waiting = false;
        DisplayLine();
    }

    void CheckForDeliverFood()
    {
        waitingToDeliverFood = !gMan.TEMP_DELIVERED;
        if (waitingToDeliverFood) return;

        waiting = false;
        DisplayLine();
    }

    void CheckForSelectRecipe()
    {
        waitingToSelectRecipe = !gMan.TEMP_SELECTED_RECIPE;
        if (waitingToSelectRecipe) return;

        waiting = false;
        DisplayLine();
    }

    void CheckDialogueStart()
    {
        waitingToStartDialogue = !DialogueManager.instance.IsDialogueActive();
        if (waitingToStartDialogue) return;

        waiting = false;
        DisplayLine();
    }

    void CheckTakeOrder()
    {
        waitingToTakeOrder = !gMan.orderController.TEMP_HAS_ORDER;
        if (waitingToTakeOrder) return;

        waiting = false;
        DisplayLine();
    }


    void CheckWASD()
    {
        if (Input.GetKeyDown(KeyCode.W) && !WASDstring.Contains("w")) WASDstring += "w";
        if (Input.GetKeyDown(KeyCode.A) && !WASDstring.Contains("a")) WASDstring += "a";
        if (Input.GetKeyDown(KeyCode.S) && !WASDstring.Contains("s")) WASDstring += "s";
        if (Input.GetKeyDown(KeyCode.D) && !WASDstring.Contains("d")) WASDstring += "d";
        bool done = WASDstring.Contains("w") && WASDstring.Contains("a") && WASDstring.Contains("s") && WASDstring.Contains("d");
        if (!done) return;

        waitingForWASD = false;
        waiting = false;
        DisplayLine();
    }
}
