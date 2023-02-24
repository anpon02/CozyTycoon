using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] float patience;
    [SerializeField] float eatTime = 5;
    [SerializeField] private Item favoriteEntree;
    [SerializeField] private float favoriteItemChance = 80.0f;

    private CustomerCoordinator custCoordinator;
    ChefController chef;
    CustomerMovement move;
    Item desiredItem;
    bool foodOrdered;
    bool recievedFood;
    float timeSinceOrdering;
    float timeSinceReceivedFood;
    bool doneSpeaking, setOrder;
    bool foodAte;
    DialogueManager dMan;

    private void Awake() {
        custCoordinator = GetComponentInParent<CustomerCoordinator>();
        move = GetComponentInParent<CustomerMovement>();
        recievedFood = false;
        foodAte = false;
<<<<<<< HEAD
        //print("this is literally just here for the meta file to be pushed. we don't like the meta file. down with the meta file.");
=======
>>>>>>> 977418fb08b5906a7cfcfb0e8458a6a743028df4
    }

    private void Start()
    {
        dMan = DialogueManager.instance;
        if (dMan) dMan.OnDialogueEnd.AddListener(CheckToLeave);
    }
    
    private void Update()
    {
        if (foodOrdered && !recievedFood) timeSinceOrdering += Time.deltaTime;
        if (recievedFood && !foodAte) Eat();
    }

    public void Order()
    {
        if (!setOrder) {
            var menu = RecipeManager.instance.Menu;
            if (menu.Count == 0) return;

            // order favorite entree if possible and random chance is hit, otherwise, order random item
            if(favoriteEntree != null && menu.Contains(favoriteEntree) && Random.Range(0, 101) <= favoriteItemChance)
                desiredItem = favoriteEntree;
            else
                desiredItem = menu[Random.Range(0, menu.Count)];
        }
        
        setOrder = false;    
        foodOrdered = true;
        timeSinceOrdering = 0;
        if (GameManager.instance.orderController) GameManager.instance.orderController.Order(desiredItem, patience, custCoordinator.characterName);
        CustomerManager.instance.GoToTable(transform);
    }

    public void DeliverFood()
    {
        if (!chef) chef = KitchenManager.instance.chef;
        if (!CorrectFoodHeld()) return;

        foodOrdered = false;
        recievedFood = true;
        foodAte = false;
        timeSinceReceivedFood = 0;

        Item deliveredItem = chef.RemoveHeldItem();
        GameManager.instance.orderController.CompleteOrder(custCoordinator.characterName);

        GameManager.instance.TEMP_DELIVERED = true;
        var affection = UpdateAffection();
        GameManager.instance.wallet.money += deliveredItem.value * (1 + affection);
    }

    public void SetOrder(Item order)
    {
        desiredItem = order;
        setOrder = true;
    }

    public void UnsetOrder()
    {
        setOrder = false;
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
        if(timeSinceOrdering < patience * (1.0f / 3.0f)) points += 3;
        else if(timeSinceOrdering < patience * (2.0f / 3.0f)) points += 2;
        else if(timeSinceOrdering < patience) points += 1;

        custCoordinator.updateRelationshipValue(points);
        return points;
    }

    void CheckToLeave()
    {
        if (dMan.lastSpeaker != custCoordinator.characterName || timeSinceOrdering < eatTime || !recievedFood) return;
        move.LeaveRestaurant();
    }

    void Eat()
    {
        timeSinceReceivedFood += Time.deltaTime;
        bool speaking = dMan.IsDialogueActive() && dMan.lastSpeaker == custCoordinator.characterName;
        if (!speaking && timeSinceReceivedFood >= eatTime) { 
            CustomerManager.instance.MakeCustomerLeave(custCoordinator.characterName);
            foodAte = true;
        }
    }

    bool CorrectFoodHeld()
    {
        if (!GetChef() || chef.GetHeldItem() == null || !chef.GetHeldiCoord().plated) return false;
        return chef.GetHeldItem().Equals(desiredItem);
    }

    bool GetChef()
    {
        if (chef) return true;
        if (!KitchenManager.instance) return false;
        chef = KitchenManager.instance.chef;
        return chef != null;
    }

    public float GetPatience() {
        return patience;
    }
}