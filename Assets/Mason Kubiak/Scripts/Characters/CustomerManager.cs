using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private List<Transform> customers;
    [SerializeField] private List<Transform> tables;

    private List<Transform> potentialCustomers;
    private List<Transform> potentialTables;
    
    [Header("DEBUG")]
    [SerializeField] private bool dayIsOver;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        potentialTables = new List<Transform>();
    }

    private void Start() {
        GameManager.instance.OnStoreOpen.AddListener(WakeUp);
        GameManager.instance.OnStoreClose.AddListener(EveryoneLeave);
    }

    public void WakeUp() {
        // reset each character
        foreach(Transform customer in customers) {
            customer.GetComponentInChildren<CustomerOrderController>().SetHasReceivedFood(false);
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
            if(dayIsOver) yield break;  // dayIsOver will eventually be swapped out for soemthing in the day/night cycle

            //if there are any potential tables
            CalculatePotentialTables();
            if(potentialTables.Count > 0) {
                // pick a random customer. if they can be seated, remove them from the list and seat them
                int customerChoice = Random.Range(0, potentialCustomers.Count);
                if(potentialCustomers[customerChoice].GetComponent<CustomerMovement>().ComeToEat(potentialTables))
                    potentialCustomers.RemoveAt(customerChoice);
            }
        }
    }
}
