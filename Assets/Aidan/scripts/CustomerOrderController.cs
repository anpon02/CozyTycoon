using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(RelationshipStatus))]
public class CustomerOrderController : MonoBehaviour
{
    Item desiredItem;
    [SerializeField] float patience;
    [SerializeField] float eatTime = 5;

    [HideInInspector] public bool storyStarted;
    [Header("Debug Tools")]
    [SerializeField] private bool testCustomer;
    [Range(0f, 1f)]
    [SerializeField] private float foodValue;

    private CustomerCoordinator custCoordinator;
    ChefController chef;
    CustomerMovement move;
    bool foodOrdered;
    bool recievedFood;
    float timeSinceOrdering;
    float timeSinceReceivedFood;
    bool doneSpeaking;
    DialogueManager dMan;

    public void Order()
    {
        var menu = RecipeManager.instance.Menu;
        if (menu.Count == 0) return;
        desiredItem = menu[Random.Range(0, menu.Count)];

        storyStarted = false;
        foodOrdered = true;
        timeSinceOrdering = 0;
        print(custCoordinator);
        if (GameManager.instance.orderController) GameManager.instance.orderController.Order(desiredItem, patience, custCoordinator.characterName);
        CustomerManager.instance.GoToTable(transform);
    }

    public void DeliverFood()
    {
        if (!chef) chef = KitchenManager.instance.chef;
        if (!CorrectFoodHeld()) return;

        foodOrdered = false;
        recievedFood = true;
        timeSinceReceivedFood = 0;

        Item deliveredItem = chef.RemoveHeldItem();
        GameManager.instance.orderController.CompleteOrder(custCoordinator.characterName);

        GameManager.instance.TEMP_DELIVERED = true;
        var affection = UpdateAffection();
        GameManager.instance.wallet.money += deliveredItem.value * (1 + affection);
    }

    public bool alreadyOrdered()
    {
        return foodOrdered;
    }

    public bool GetHasReceivedFood()
    {
        return recievedFood;
    }

    public void SetHasReceivedFood(bool received)
    {
        recievedFood = received;
    }

    int UpdateAffection()
    {
        int points = 0;
        if (timeSinceOrdering < patience) points += 1;
        if (!storyStarted) points = 0;

        custCoordinator.updateRelationshipValue(points);

        return points;
    }

    private void Start()
    {
        dMan = DialogueManager.instance;
        if (dMan) dMan.OnDialogueEnd.AddListener(CheckToLeave);
    }

    void CheckToLeave()
    {
        if (dMan.lastSpeaker != custCoordinator.characterName || timeSinceOrdering < eatTime || !recievedFood) return;
        move.LeaveRestaurant();
    }

    private void Update()
    {
        print(custCoordinator);
        if (foodOrdered && !recievedFood) timeSinceOrdering += Time.deltaTime;
        if (recievedFood) Eat();
    }

    void Eat()
    {
        timeSinceReceivedFood += Time.deltaTime;
        bool speaking = dMan.IsDialogueActive() && dMan.lastSpeaker == custCoordinator.characterName;
        if (!speaking && timeSinceReceivedFood >= eatTime) move.LeaveRestaurant();
    }

    private void Awake() {
        custCoordinator = GetComponentInParent<CustomerCoordinator>();
        print(custCoordinator);
        move = GetComponentInParent<CustomerMovement>();
        recievedFood = false;
    }

    bool CorrectFoodHeld()
    {
        if (!GetChef() || chef.GetHeldItem() == null) return false;
        return chef.GetHeldItem().Equals(desiredItem);
    }

    bool GetChef()
    {
        if (chef) return true;
        if (!KitchenManager.instance) return false;
        chef = KitchenManager.instance.chef;
        return chef != null;
    }
}
