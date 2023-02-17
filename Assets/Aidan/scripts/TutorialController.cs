using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    }

    [SerializeField] TextMeshProUGUI mainText, category, details;
    [SerializeField] GameObject shop, shopButton, recipeBook, recipeButton;
    [SerializeField] Item mixer, potato;
    [SerializeField] float offset, waitTime = 1f;
    [SerializeField] Helper helpScript;
    [SerializeField] List<Instruction> instructions = new List<Instruction>();
    [SerializeField] bool startFromBeginning;
    int currentInstruction;

    GameManager gMan;
    KitchenManager kMan;
    string WASDstring = "";
    GameObject luca;



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

        //category.text = "";
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
            kMan.NextTutSection();
            return;
        }

        AudioManager.instance.PlaySound(5, gameObject);

        mainText.text = instructions[currentInstruction].text;
        category.text = instructions[currentInstruction].category;
        details.text = instructions[currentInstruction].subTitle;
    }

    void EndTut()
    {
        AudioManager.instance.PlaySound(6);
        gameObject.SetActive(false);
        helpScript.gameObject.SetActive(false);
    }

    void Update()
    {
        if (instructions[currentInstruction].complete) { currentInstruction += 1; DisplayLine(); }
        if (instructions.Count <= currentInstruction) return;

        DisplayHelpers();
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

    void DisplayHelpers()
    {
        if (string.IsNullOrEmpty(category.text)) return;
        CrossScenePointer();
        if (instructions[currentInstruction].pointer == null) { helpScript.gameObject.SetActive(false); return; }

        helpScript.gameObject.SetActive(true);
        helpScript.worldPosTarget = instructions[currentInstruction].pointer.transform.position + Vector3.up * offset;
    }

    void CrossScenePointer()
    {
        if (!kMan) return;
        luca = CustomerManager.instance.transform.GetChild(0).gameObject;
        instructions[5].pointer = luca;
        instructions[7].pointer = luca;
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
