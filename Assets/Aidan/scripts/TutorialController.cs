using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [System.Serializable]
    public class Instruction
    {
        public string text;
        public string subTitle;
        public string category;
        public bool complete;
        public GameObject pointer;
        public Vector3 pointerOffset;
    }

    [SerializeField] TextMeshProUGUI mainText, category, details;
    [SerializeField] GameObject shop, shopButton, recipeBook, recipeButton, stove, pans, meatFridge;
    [SerializeField] Item mixer, potato, lucaOrder, fryingPan, rawChicken, pancakes;
    [SerializeField] float offset, waitTime = 1f;
    [SerializeField] Helper helpScript;
    [SerializeField] List<Instruction> instructions = new List<Instruction>();
    [SerializeField] bool startFromBeginning;
    int currentInstruction;

    GameManager gMan;
    KitchenManager kMan;
    string WASDstring = "";
    GameObject luca;
    bool pointing;

    int chickenProgress;


    private void Start()
    {
        if (startFromBeginning) {
            foreach (var i in instructions) i.complete = false;
        }
        gMan = GameManager.instance;
        kMan = KitchenManager.instance;

        
        gMan.timeScript.PauseTime();
        gMan.PauseNotifs();

        int lastCompleted = 0;
        for (int i = 0; i < instructions.Count; i++) {
            if (instructions[i].complete) lastCompleted += 1;
            else break;
        }
        currentInstruction = lastCompleted;

    }

    public void DisplayLine()
    {
        if (kMan == null) Start();
        if (instructions.Count <= currentInstruction) {EndTut(); return; }
        gameObject.SetActive(true);

        if (instructions[currentInstruction].category != category.text && category.text.Length > 1) {
            category.text = "";
            AudioManager.instance.PlaySound(6);
            helpScript.gameObject.SetActive(false);
            if (currentInstruction < 14) kMan.NextTutSection();
            else kMan.NextTutSectionTomorrow();
            return;
        }

        AudioManager.instance.PlaySound(5, gameObject);

        mainText.text = instructions[currentInstruction].text;
        category.text = instructions[currentInstruction].category;
        details.text = instructions[currentInstruction].subTitle;
    }

    public void EndTut()
    {
        AudioManager.instance.PlaySound(6);
        gameObject.SetActive(false);
        helpScript.gameObject.SetActive(false);
        gMan.UnPauseNotifs();
    }

    void Update()
    {
        if (instructions[currentInstruction].complete) { currentInstruction += 1; DisplayLine(); }
        if (instructions.Count <= currentInstruction) return;

        DisplayHelper();
        CheckWASD();
        CheckForHeldPotato();
        CheckForPlacedPotato();
        CheckForRetrievedPotato();
        CheckForTrashedPotato();

        if (currentInstruction >= 5) OpenStore();
        CheckForLine();
        CheckForOrderTaken();
        CheckForConvoStarted();
        CheckForRecipeBookOpen();
        CheckForRecipeBookClose();

        if (currentInstruction == 10) {
            pointing = true;

            if (panOnStove() && chickenInHand()) PointToStove();
            else if (panOnStove()) PointToChicken();
            else if (panInHand()) PointToStove();
            else PointToPan();

            if (panOnStove() && chickenOnStove() || instructions[11].complete) {
                pointing = false;
                instructions[10].complete = true;
            }
        }

        CheckForMinigameStart();
        CheckForMinigameComplete();
        CheckIfPlated();
        CheckIfDelivered();

        //[15]
        CheckForShopOpen();
        CheckForSpecialtyTabSelected();
        CheckForEquipmentTabSelected();
        CheckForMixerBought();
        CheckForPancakeMade();
    }

    void CheckForPancakeMade()
    {
        if (kMan.chef.IsHoldingItem() && kMan.chef.GetHeldItem().Equals(pancakes)) instructions[19].complete = true;
    }

    void CheckForMixerBought()
    {
        if (mixer.IsPresentInList(kMan.unlockedEquipment)) instructions[18].complete = true;
    }

    void CheckForEquipmentTabSelected()
    {
        if (instructions[16].complete && kMan.equipmentTabSelected) instructions[17].complete = true;
    }

    void CheckForSpecialtyTabSelected()
    {
        if (instructions[15].complete && kMan.specialtyTabSelected) instructions[16].complete = true;
    }

    void CheckForShopOpen()
    {
        if (instructions[14].complete && kMan.shopOpen) instructions[15].complete = true;
    }

    void CheckIfDelivered()
    {
        if (gMan.orderController.completedOrder) instructions[14].complete = true;
    }

    void CheckIfPlated()
    {
        if (kMan.chef.GetHeldiCoord() != null && instructions[12].complete && kMan.chef.GetHeldiCoord().plated) instructions[13].complete = true;
    }

    void CheckForMinigameStart()
    {
        if (kMan.minigameStarted) instructions[11].complete = true;
    }

    void CheckForMinigameComplete()
    {
        if (kMan.minigameCompleted) instructions[12].complete = true;
    }

    void PointToChicken()
    {
        helpScript.gameObject.SetActive(true);
        helpScript.worldPosTarget = meatFridge.transform.position + Vector3.up * offset;
    }
    void PointToStove()
    {
        helpScript.gameObject.SetActive(true);
        helpScript.worldPosTarget = stove.transform.position + Vector3.up * offset;
    }

    void PointToPan()
    {
        helpScript.gameObject.SetActive(true);
        helpScript.worldPosTarget = pans.transform.position + Vector3.up * offset;
    }

    bool chickenOnStove()
    {
        return rawChicken.IsPresentInList(stove.GetComponent<WorkspaceController>().GetItemList());
    }

    bool panOnStove()
    {
        return fryingPan.IsPresentInList(stove.GetComponent<WorkspaceController>().GetItemList());
    }

    bool chickenInHand()
    {
        return rawChicken.Equals(kMan.chef.GetHeldItem());
    }

    bool panInHand()
    {
        return fryingPan.Equals(kMan.chef.GetHeldItem());
    }

    void CheckForRecipeBookClose()
    {
        if (instructions[8].complete && !recipeBook.activeInHierarchy) instructions[9].complete = true;
    }

    void CheckForRecipeBookOpen()
    {
        if (recipeBook.activeInHierarchy) {
            instructions[8].complete = true;
            recipeButton.SetActive(true);
        }
    }

    void CheckForConvoStarted()
    {
        if (!DialogueManager.instance) return;
        if (DialogueManager.instance.ConvoStarted) instructions[7].complete = true;
    }

    void CheckForOrderTaken()
    {
        if (gMan.orderController.TEMP_HAS_ORDER) instructions[6].complete = true;
    }

    void CheckForLine()
    {
        var dist = Vector3.Distance(luca.transform.position, LineManager.instance.GetLineSpots()[0].GetPlaceCoordinates());
        if (dist <= 0.1f) instructions[5].complete = true;
    }

    void OpenStore()
    {
        if (!gMan.timeScript.paused) return;
        gMan.timeScript.time = .14f;
        gMan.timeScript.UnpauseTime();
    }

    void CheckForTrashedPotato()
    {
        if (kMan.lastTrashedItem == null) return;
        if (kMan.lastTrashedItem.Equals(potato) && instructions[3].complete) instructions[4].complete = true;
    }

    void CheckForRetrievedPotato()
    {
        if (kMan.lastRetrievedItem == null) return;
        if (kMan.lastRetrievedItem.Equals(potato) && instructions[2].complete) instructions[3].complete = true;
    }

    void CheckForPlacedPotato()
    {
        if (kMan.lastAddedItem == null) return;
        if (kMan.lastAddedItem.Equals(potato)) instructions[2].complete = true;
    }

    void DisplayHelper()
    {
        if (pointing || string.IsNullOrEmpty(category.text)) return;
        CrossScenePointer();
        if (instructions[currentInstruction].pointer == null) { helpScript.gameObject.SetActive(false); return; }

        helpScript.gameObject.SetActive(true);
        helpScript.worldPosTarget = (instructions[currentInstruction].pointer.transform.position + Vector3.up * offset) + instructions[currentInstruction].pointerOffset;
    }

    void CrossScenePointer()
    {
        if (!kMan) return;
        luca = CustomerManager.instance.transform.GetChild(0).gameObject;
        if (!instructions[14].complete) luca.GetComponentInChildren<CustomerOrderController>().SetOrder(lucaOrder);
        else luca.GetComponentInChildren<CustomerOrderController>().UnsetOrder();
        instructions[5].pointer = luca;
        instructions[6].pointer = luca;
        instructions[7].pointer = luca;
        instructions[14].pointer = luca;
    }

    void CheckForHeldPotato()
    {
        if (kMan.chef.IsHoldingItem() && kMan.chef.GetHeldItem().Equals(potato)) instructions[1].complete = true;
    }
    
    void CheckWASD()
    {
        if (Input.GetKeyDown(KeyCode.W) && !WASDstring.Contains("w")) WASDstring += "w";
        if (Input.GetKeyDown(KeyCode.A) && !WASDstring.Contains("a")) WASDstring += "a";
        if (Input.GetKeyDown(KeyCode.S) && !WASDstring.Contains("s")) WASDstring += "s";
        if (Input.GetKeyDown(KeyCode.D) && !WASDstring.Contains("d")) WASDstring += "d";
        instructions[0].complete = WASDstring.Contains("w") && WASDstring.Contains("a") && WASDstring.Contains("s") && WASDstring.Contains("d");
    }
}
