using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScheduleDay {
    [Tooltip("This is purely for organization in the editor")]
    public string dayName;      // this is purely for organization in the editor
    public List<CharacterName> customers;
}

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private List<Transform> customers;
    //[SerializeField] private List<Transform> tables;
    [SerializeField] private List<ScheduleDay> schedule;

    private List<Transform> todaysCustomers;
    //private List<Transform> potentialCustomers;
    //private List<Transform> potentialTables;
    private List<Transform> customersInLine;

    private GameManager gMan;

    public float todaysCombinedPatience;
    
    [Header("DEBUG")]
    [SerializeField] private bool dayIsOver;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        todaysCustomers = new List<Transform>();
        //potentialTables = new List<Transform>();
        customersInLine = new List<Transform>();
    }

    private void Start() {
        gMan = GameManager.instance;
        gMan.OnStoreOpen.AddListener(WakeUp);
        gMan.OnStoreClose.AddListener(EveryoneLeave);
    }

    private void SetTodaysCustomers() {
        foreach(Transform customer in customers) {
            CustomerCoordinator coord = customer.GetComponent<CustomerCoordinator>();
            if(schedule[gMan.timeScript.day].customers.Contains(coord.characterName))
                todaysCustomers.Add(customer);
        }
        print("TODAYS CUSTOMERS");
    }

    private void CalculateCombinedPatience() {
        float sum = 0;
        foreach(Transform customer in todaysCustomers) {
            CustomerOrderController ordCont = customer.GetComponentInChildren<CustomerOrderController>();
            sum += ordCont.GetPatience();
        }
        todaysCombinedPatience = sum;
    }

    public void MakeCustomerLeave(CharacterName character)
    {
        foreach (var c in customers) {
            CustomerCoordinator coord = c.GetComponent<CustomerCoordinator>();
            if (coord.characterName == character) {
                c.GetComponent<CustomerMovement>().LeaveRestaurant();
                coord.inRestaurant = false;
            }
        }
    }

    public void WakeUp() {
        print("WAKEUP");
        // reset each character
        foreach(Transform customer in customers) {
            customer.GetComponentInChildren<CustomerOrderController>().SetHasReceivedFood(false);
            customer.GetComponent<CustomerCoordinator>().SetStorySaid(false);
        }

        // start customers coming
        //potentialCustomers = new List<Transform>(customers);
        SetTodaysCustomers();
        CalculateCombinedPatience();
        StartCoroutine("StartSendingCustomers");
    }

    public void EveryoneLeave() {
        StopCoroutine("StartSendingCustomers");
        foreach(Transform customer in customers) {
            CustomerCoordinator coord = customer.GetComponent<CustomerCoordinator>();
            if(coord.inRestaurant) {
                customer.GetComponent<CustomerMovement>().LeaveRestaurant();
                coord.inRestaurant = false;
            }
        }
    }
/*
    private void CalculatePotentialTables() {
        potentialTables.Clear();

        // loop through and add any open tables to potentialTables
        foreach(Transform table in tables)
            if(!table.GetComponent<Table>().GetIsTaken())
                potentialTables.Add(table);
    }
*/
    private IEnumerator StartSendingCustomers() {
        /*
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
        */
        print("SENDING");
        for(int i = 0; i < todaysCustomers.Count; ++i) {
            if(i > 0) {
                yield return new WaitUntil(() => !CustomerInRestaurant(i - 1));
            }
            todaysCustomers[i].GetComponent<CustomerCoordinator>().inRestaurant = true;
            todaysCustomers[i].GetComponent<CustomerMovement>().GetInLine();
        }
        //yield return new WaitUntil(())
        print("DONE SENDING");
    }

    private bool CustomerInRestaurant(int customerIndex) {
        return todaysCustomers[customerIndex].GetComponent<CustomerCoordinator>().inRestaurant;
    }

    private IEnumerator ShiftLine() {
        foreach(Transform customer in customersInLine) {
            yield return new WaitForSeconds(0.1f);
            customer.GetComponentInChildren<CustomerMovement>().MoveUpInLine();
        }
    }

    // we do not need to calculate potential tables anymore
    public void GoToTable(Transform customer) {
        //CalculatePotentialTables();
        //if(potentialTables.Count > 0) {
        //customer.GetComponentInParent<CustomerMovement>().ComeToEat(potentialTables);
        customer.GetComponentInParent<CustomerMovement>().ComeToEat();
        customersInLine.Remove(customer.parent);
        StartCoroutine("ShiftLine");
        //}
    }
}
