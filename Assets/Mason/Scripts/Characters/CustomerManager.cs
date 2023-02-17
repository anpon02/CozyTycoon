using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScheduleDay {
    public string dayName;      // this is purely for organization in the editor
    public List<CharacterName> customers;
}

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private List<Transform> customers;
    [SerializeField] private List<Transform> tables;
    [SerializeField] private List<ScheduleDay> schedule;

    private List<Transform> potentialCustomers;
    private List<Transform> potentialTables;
    private List<Transform> customersInLine;
    
    [Header("DEBUG")]
    [SerializeField] private bool dayIsOver;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        potentialTables = new List<Transform>();
        customersInLine = new List<Transform>();
    }

    private void Start() {
        GameManager.instance.OnStoreOpen.AddListener(WakeUp);
        GameManager.instance.OnStoreClose.AddListener(EveryoneLeave);
    }

    public void MakeCustomerLeave(CharacterName character)
    {
        foreach (var c in customers) {
            if (c.GetComponentInChildren<CustomerStory>().characterName == character) 
                c.GetComponent<CustomerMovement>().LeaveRestaurant();
        }
    }

    public void WakeUp() {

        // reset each character
        foreach(Transform customer in customers) {
            customer.GetComponentInChildren<CustomerOrderController>().SetHasReceivedFood(false);
            customer.GetComponent<CustomerCoordinator>().SetStorySaid(false);
        }

        // start customers coming
        potentialCustomers = new List<Transform>(customers);
        StartCoroutine("StartSendingCustomers");
    }

    public void EveryoneLeave() {
        StopCoroutine("StartSendingCustomers");
        foreach(Transform customer in customers) {
            customer.GetComponent<CustomerMovement>().LeaveRestaurant();
        }
    }

    private void CalculatePotentialTables() {
        potentialTables.Clear();

        // loop through and add any open tables to potentialTables
        foreach(Transform table in tables)
            if(!table.GetComponent<Table>().GetIsTaken())
                potentialTables.Add(table);
    }

    private IEnumerator StartSendingCustomers() {
        while(potentialCustomers.Count > 0) {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            AudioManager.instance.PlaySound(14);
            if(dayIsOver) yield break;  // dayIsOver will eventually be swapped out for soemthing in the day/night cycle

            // pick random customer to send to line
            int customerChoice = Random.Range(0, potentialCustomers.Count);
            potentialCustomers[customerChoice].GetComponent<CustomerMovement>().GetInLine();
            customersInLine.Add(potentialCustomers[customerChoice]);
            potentialCustomers.RemoveAt(customerChoice);
        }
    }

    private IEnumerator ShiftLine() {
        foreach(Transform customer in customersInLine) {
            yield return new WaitForSeconds(0.1f);
            customer.GetComponentInChildren<CustomerMovement>().MoveUpInLine();
        }
    }

    // we do not need to calculate potential tables anymore
    public void GoToTable(Transform customer) {
        CalculatePotentialTables();
        if(potentialTables.Count > 0) {
            customer.GetComponentInParent<CustomerMovement>().ComeToEat(potentialTables);
            customersInLine.Remove(customer.parent);
            StartCoroutine("ShiftLine");
        }
    }
}
