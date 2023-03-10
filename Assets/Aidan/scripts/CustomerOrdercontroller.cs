using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] float patience;
    [SerializeField] float eatTime = 5;
    [SerializeField] private float sideOrderChance = 50.0f;

    [Header("Favorites")]
    [SerializeField] private float favoriteItemChance = 80.0f;
    [SerializeField] private Item superFavorite;
    [SerializeField] private List<Item> favoriteItems;

    private CustomerCoordinator custCoordinator;
    ChefController chef;
    CustomerMovement move;
    List<Item> orderedItems;
    List<Item> lastOrder;
    bool recievedFood;
    float timeSinceOrdering;
    float timeSinceReceivedFood;
    bool doneSpeaking;
    public bool setOrder { get; private set; }
    bool foodAte;
    DialogueManager dMan;

    private void Awake() {
        custCoordinator = GetComponentInParent<CustomerCoordinator>();
        move = GetComponentInParent<CustomerMovement>();
        orderedItems = new List<Item>();
        recievedFood = false;
        foodAte = false;
    }

    private void Start()
    {
        dMan = DialogueManager.instance;
        if (dMan) dMan.OnDialogueEnd.AddListener(CheckToLeave);
    }
    
    private void Update()
    {
        if(orderedItems.Count > 0 && !recievedFood) timeSinceOrdering += Time.deltaTime;
        if (recievedFood && !foodAte) Eat();
    }

    private Item PickItem(List<Item> menu) {
        Item selectedItem;
        do {
            selectedItem = menu[Random.Range(0, menu.Count)];
        } while(lastOrder != null && lastOrder.Contains(selectedItem) && orderedItems.Contains(selectedItem));
        return selectedItem;
    }

    private void NormalOrder(List<Item> entrees, List<Item> sides) {
        // pick entree
        if (!setOrder && entrees.Count > 0) {
            // pick favorite item or random item
            List<Item> availableFavorites = GetAvailableFavorites(entrees);
            if(availableFavorites.Count > 0 && Random.Range(0, 101) <= favoriteItemChance)
                orderedItems.Add(PickItem(availableFavorites));
            else {
                // never select same entree twice
                orderedItems.Add(PickItem(entrees));
            }
        }

        // chance to pick side if possible
        if(!setOrder && sides.Count > 0 && !orderedItems[0].side && Random.Range(0, 101) <= sideOrderChance) {
            // never select same side twice
            orderedItems.Add(PickItem(sides));
        }
    }

    public void Order()
    {
        var entreeMenu = RecipeManager.instance.Menu;
        var sideMenu = RecipeManager.instance.Sides;

        // pick super item + side if available, otherwise order normally
        if(superFavorite != null && entreeMenu.Contains(superFavorite) && !setOrder) {
            orderedItems.Add(superFavorite);
            orderedItems.Add(PickItem(sideMenu));
        }
        else
            NormalOrder(entreeMenu, sideMenu);

        // process order
        if(orderedItems.Count > 0) {
            setOrder = false;  
            timeSinceOrdering = 0;
            lastOrder = new List<Item>(orderedItems);

            // order
            if (GameManager.instance.orderController) {
                if(orderedItems.Count == 1)
                    GameManager.instance.orderController.Order(orderedItems[0], custCoordinator.characterName);
                else
                    GameManager.instance.orderController.Order(orderedItems[0], custCoordinator.characterName, orderedItems[1]);
            }
            CustomerManager.instance.GoToTable(transform);
        }
    }

    public void DeliverFood()
    {
        if (!chef) chef = KitchenManager.instance.chef;
        if (!CorrectFoodHeld()) return;

        orderedItems.Clear();
        recievedFood = true;
        foodAte = false;
        timeSinceReceivedFood = 0;

        Item deliveredItem = chef.RemoveHeldItem();
        GameManager.instance.orderController.CompleteOrder(custCoordinator.characterName);

        GameManager.instance.TEMP_DELIVERED = true;
        var affection = UpdateAffection();
        GameManager.instance.wallet.money += Mathf.RoundToInt(deliveredItem.value * (1 + affection/3.0f));
    }

    public void SetOrder(Item order, Item side = null)
    {
        if(orderedItems.Count == 0) {
            orderedItems.Add(order);
            if(side != null)
                orderedItems.Add(side);
            setOrder = true;
        }
    }

    public void UnsetOrder()
    {
        setOrder = false;
    }

    public bool alreadyOrdered()
    {
        return orderedItems.Count > 0;
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
        if(orderedItems.Count == 1) {
            return chef.GetHeldItem().Equals(orderedItems[0]) && chef.GetHeldiCoord().side == null;
        }
        else if(orderedItems.Count == 2 && chef.GetHeldiCoord().side != null) {
            return chef.GetHeldItem().Equals(orderedItems[0]) && chef.GetHeldiCoord().side.Equals(orderedItems[1]);
        }
        return false;
    }

    bool GetChef()
    {
        if (chef) return true;
        if (!KitchenManager.instance) return false;
        chef = KitchenManager.instance.chef;
        return chef != null;
    }

    private List<Item> GetAvailableFavorites(List<Item> menu) {
        List<Item> availableFavorites = new List<Item>();
        foreach(Item i in favoriteItems) {
            if(menu.Contains(i))
                availableFavorites.Add(i);
        }
        return availableFavorites;
    }

    public float GetPatience() {
        return patience;
    }
}