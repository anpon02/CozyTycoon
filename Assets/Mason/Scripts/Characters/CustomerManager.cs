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

    [SerializeField] private List<Transform> customers;
    [SerializeField] private List<ScheduleDay> schedule;

    [HideInInspector] public float todaysCombinedPatience;
    private List<Transform> todaysCustomers;
    private List<Transform> customersInLine;
    private GameManager gMan;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        todaysCustomers = new List<Transform>();
        customersInLine = new List<Transform>();
    }

    private void Start() {
        gMan = GameManager.instance;
        gMan.OnStoreOpen.AddListener(WakeUp);
        gMan.OnStoreClose.AddListener(EveryoneLeave);
    }

    private void SetTodaysCustomers() {
        todaysCustomers.Clear();
        if (schedule.Count <= gMan.timeScript.day) return;
        foreach (var customer in schedule[gMan.timeScript.day].customers) {
            Transform c = customers.Find( c => c.GetComponent<CustomerCoordinator>().characterName == customer);
            if (c != null) {
                todaysCustomers.Add(c);
            }
        }
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
                CustomerMovement move = c.GetComponent<CustomerMovement>();
                move.LeaveRestaurant();
            }
        }
    }

    public void WakeUp() {
        // reset each character
        foreach(Transform customer in customers) {
            customer.GetComponentInChildren<CustomerOrderController>().SetHasReceivedFood(false);
            customer.GetComponent<CustomerCoordinator>().SetStorySaid(false);
            customer.GetComponent<CustomerMovement>().currentSpotInLine = -1;
        }

        // start customers coming
        SetTodaysCustomers();
        CalculateCombinedPatience();
        StartCoroutine("StartSendingCustomers");
    }

    public void EveryoneLeave() {
        StopCoroutine("StartSendingCustomers");
        foreach(Transform customer in customers) {
            CustomerMovement move = customer.GetComponent<CustomerMovement>();
            if(move.inRestaurant) {
                move.LeaveRestaurant();
            }
        }
    }

    private IEnumerator StartSendingCustomers() {
        for(int i = 0; i < todaysCustomers.Count; ++i) {
            if (i > 0)
                if(DialogueManager.instance.StoryDisabled(todaysCustomers[i - 1].GetComponent <CustomerCoordinator>().characterName))
                    yield return new WaitUntil(() => todaysCustomers[i - 1].GetComponent<CustomerMovement>().InLine() );
                else yield return new WaitUntil(() =>  CustomerFinishedTalking(i - 1));
            CustomerMovement move = todaysCustomers[i].GetComponent<CustomerMovement>();
            move.GetInLine();
        }
        //print("AHHHH: " + todaysCustomers.Count);
        yield return new WaitUntil(() => !CustomerInRestaurant(todaysCustomers.Count - 1));
        //print("BRUHHH");
        gMan.timeScript.LastCustomerLeave();
    }

    bool CustomerFinishedTalking(int customerIndex)
    {
        return todaysCustomers[customerIndex].GetComponent<CustomerCoordinator>().storyFinished;
    }

    private bool CustomerInRestaurant(int customerIndex) {
        return todaysCustomers[customerIndex].GetComponent<CustomerMovement>().inRestaurant;
    }

    private IEnumerator ShiftLine() {
        foreach(Transform customer in customersInLine) {
            yield return new WaitForSeconds(0.1f);
            customer.GetComponentInChildren<CustomerMovement>().MoveUpInLine();
        }
    }

    public void GoToTable(Transform customer) {
        customer.GetComponentInParent<CustomerMovement>().ComeToEat();
        customersInLine.Remove(customer.parent);
        StartCoroutine("ShiftLine");
    }
}
