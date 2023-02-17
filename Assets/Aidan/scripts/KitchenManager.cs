using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour {
    public static KitchenManager instance;
    void Awake() { instance = this; }

    [System.Serializable]
    public class ProductObject {
        public Product product;
        public Item item;
    }

    [SerializeField] GameObject itemCoordPrefab;
    [SerializeField] List<ProductObject> productObjData = new List<ProductObject>();
    public List<Item> unlockedEquipment = new List<Item>();
    public List<Item> unlockedIngredients = new List<Item>();
    [HideInInspector] public List<ItemStorage> allStorage = new List<ItemStorage>();
    public Sprite genericVeggies, genericMeat, genericBread;
    public float playerReach = 5, tutorialStartTime = 2;
    [SerializeField] TutorialController tutorial;
    [HideInInspector] public bool tutorialEquipmentPause;

    [HideInInspector] public ToolipCoordinator ttCoord;
    [HideInInspector] public WorkspaceController hoveredController;
    [HideInInspector] public ChefController chef;
    bool enabledEquipment;
    [HideInInspector] public Item lastAddedItem, lastRetrievedItem, lastTrashedItem;
    [HideInInspector] public bool minigameStarted, minigameCompleted;

    private void Start()
    {
        //tutorialEquipmentPause = true;
        //RecipeManager.instance.tutorialRecipeLimit = true;
        NextTutSection();
    }

    public void NextTutSection()
    {
        StartCoroutine(startTutorial());
    }

    IEnumerator startTutorial()
    {
        tutorial.gameObject.SetActive(false);
        yield return new WaitForSeconds(tutorialStartTime);
        tutorial.gameObject.SetActive(true);
        tutorial.DisplayLine();
    }

    private void Update()
    {
        if (!enabledEquipment && allStorage.Count > 0) EnableStartingEquipment();
    }

    void EnableStartingEquipment()
    {
        if (tutorialEquipmentPause) return;
        //print("ENABLED");

        enabledEquipment = true;
        var list = new List<Item>(unlockedEquipment);
        unlockedEquipment.Clear();
        foreach (var e in list) EnableEquipment(e);
    }

    public void PurchaseProduct(Product product)
    {
        foreach (var p in productObjData) {
            if (p.product.name == product.name) {
                if (p.item) EnableEquipment(p.item);
            }
        }
    }
    
    public void EnableEquipment(Item newEquipment)
    {
        if (unlockedEquipment.Contains(newEquipment)) return;

        unlockedEquipment.Add(newEquipment);
        foreach (var s in allStorage) {
            s.Enable(newEquipment);
        }
    }

    public void EnableIngredient(Item newIngredient)
    {
        if (unlockedIngredients.Contains(newIngredient)) return;

        unlockedIngredients.Add(newIngredient);
        foreach (var s in allStorage) {
            s.Enable(newIngredient);
        }
    }

    public ItemCoordinator CreateNewItemCoord(Item item, Vector3 pos)
    {
        var newGO = Instantiate(itemCoordPrefab, pos, Quaternion.identity);
        var coordScript = newGO.GetComponent<ItemCoordinator>();
        coordScript.SetItem(item);
        newGO.AddComponent<Rigidbody2D>();
        newGO.GetComponent<Rigidbody2D>().isKinematic = true;
        return coordScript;
    }
}
