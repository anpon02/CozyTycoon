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
    }

    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] GameObject shop;
    [SerializeField] Item mixer;
    [SerializeField] float offset;
    [SerializeField] Item potato;

    [SerializeField] List<Instruction> instructions = new List<Instruction>();
    int currentInstruction;

    GameManager gMan;
    KitchenManager kMan;
    string WASDstring = "";



    private void Start()
    {
        gMan = GameManager.instance;
        kMan = KitchenManager.instance;
        DisplayLine();
    }
    public void DisplayLine()
    {
        promptText.text = instructions[currentInstruction].text;
    }

    void Update()
    {
        if (instructions[currentInstruction].complete) { currentInstruction += 1; DisplayLine(); }

        DisplayHelpers();
        CheckWASD();
        CheckForHeldPotato();
    }

    void DisplayHelpers()
    {
        //if (currentInstruction == 1)
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
