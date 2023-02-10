using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderListItemCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Color completeCol;
    [SerializeField] Image timer;
    [SerializeField] bool TESTBOOL;
    [SerializeField] GameObject strikeThrough;
    public float patience;
    [HideInInspector] public CharacterName character;

    OrderUICoordinator uiCoord;
    float timeLeft;
    string itemName;
    bool red;
    

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateCircle();
        if (timeLeft <= 0 && red) FailCustomer(); 
    }

    void UpdateCircle()
    {
        timer.fillAmount = Mathf.Clamp01(timeLeft / patience);
        if (timeLeft <= 0 && !red) {
            red = true;
            timeLeft = patience;
            timer.color = Color.red;
        }
    }

    void FailCustomer()
    {
        var cMan = CustomerManager.instance;
        if (cMan) cMan.MakeCustomerLeave(character);
    }

    public void Init(string _text, float time, CharacterName _customer)
    {
        character = _customer;
        red = false;
        label.text = _text;
        itemName = _text;
        strikeThrough.SetActive(false);
        patience = timeLeft = time;

    }

    public void MarkComplete()
    {
        strikeThrough.SetActive(true);
        label.color = completeCol;
        label.fontStyle = FontStyles.Italic;
    }

    public string GetItemName()
    {
        return itemName;
    }
}
