using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour {
    public static KitchenManager instance;
    void Awake() { instance = this; }

    [System.Serializable] public class ProductObject {
        [HideInInspector] public string name;
        public Product product;
        public Item item;
    }
    [System.Serializable] public class ChoiceData
    {
        [System.Serializable]
        public class Option
        {
            public Product product;
            public  string quote;
            public Sprite characterSprite;
            public CharacterName character;
        }

        [HideInInspector] public string name;
        public List<Option> options;
        public int dayNum = 0;

        public void OnValidate()
        {
            string s = "";
            foreach (var o in options) s += o.product.productName + ", ";
            s = s.TrimEnd();
            s = s.TrimEnd(',');
            name = "day " + dayNum + ": " + s;
        }
    }


    [SerializeField] GameObject itemCoordPrefab;
    public float playerReach = 5;
    public List<Item> unlockedEquipment = new List<Item>();

    [SerializeField] List<ProductObject> productObj = new List<ProductObject>();
    
    [Header("Tutorial"), SerializeField] TutorialController tutorial; 
    [SerializeField] float tutorialStartTime = 2;
    [SerializeField] GameObject recipeButton;
    [SerializeField] bool playTutorial;

    [Header("Choices")]
    [SerializeField] List<ChoiceData> allChoices;
    [SerializeField] ChoiceController choiceController;

    [HideInInspector] public List<Item> unlockedIngredients = new List<Item>();
    [HideInInspector] public List<ItemStorage> allStorage = new List<ItemStorage>();
    [HideInInspector] public bool tutorialEquipmentPause;
    [HideInInspector] public ToolipCoordinator ttCoord;
    [HideInInspector] public WorkspaceController hoveredController;
    [HideInInspector] public ChefController chef;
    [HideInInspector] public Item lastAddedItem, lastRetrievedItem, lastTrashedItem;
    [HideInInspector] public bool minigameStarted, minigameCompleted, specialtyTabSelected, equipmentTabSelected;

    bool enabledEquipment;
    float nextTutorialMoneyAmount = Mathf.Infinity;
    List<CharacterName> chosenThisWeek = new List<CharacterName>();

    private void OnValidate()
    {
        foreach (var c in allChoices) c.OnValidate();
    }

    private void Start() 
    {
        if (playTutorial) NextTutSection();
        else {
            recipeButton.SetActive(true);
            GameManager.instance.UnPauseNotifs();
        }
        GameManager.instance.OnStoreClose.AddListener(CheckForChoice);
    }

    void CheckForChoice()
    {
        int daynum = GameManager.instance.timeScript.day + 1;
        foreach (var choice in allChoices) {
            if (choice.dayNum == daynum) {
                GameManager.instance.timeScript.PauseTime();

                choice.options = RemoveAlreadySelected(choice.options);
                if (choice.options.Count <= 0) return;

                choiceController.OpenChoiceUI(choice.options);
                break;
            }
        }
    }
    
    List<ChoiceData.Option> RemoveAlreadySelected(List<ChoiceData.Option> old)
    {
        var newList = new List<ChoiceData.Option>();

        foreach (var o in old) {
            var productObj = getObjFromProduct(o.product);

            if (productObj != null && !productObj.item.IsPresentInList(unlockedEquipment)) newList.Add(o);
        }

        return newList;
    }

    public void NextTutSection()
    {
        StartCoroutine(startTutorial());
    }

    public void nextTutSectionWhenThisRich(int price)
    {
        tutorial.gameObject.SetActive(false);
        nextTutorialMoneyAmount = price;
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
        if (GameManager.instance.wallet.money > nextTutorialMoneyAmount && GameManager.instance.timeScript.day >= 1) {
            nextTutorialMoneyAmount = Mathf.Infinity;
            StartCoroutine(startTutorial());
        }
    }

    void EnableStartingEquipment()
    {
        if (tutorialEquipmentPause) return;

        enabledEquipment = true;
        var list = new List<Item>(unlockedEquipment);
        unlockedEquipment.Clear();
        foreach (var e in list) EnableEquipment(e);
    }

    public void PurchaseProduct(Product product, CharacterName character)
    {
        chosenThisWeek.Add(character);
        if (GameManager.instance.timeScript.day+1 % 6 == 0) DisableCharacters();

        PurchaseProduct(product);
    }
    public void PurchaseProduct(Product product, bool equipment = true)
    {
        GameManager.instance.timeScript.UnpauseTime();
        choiceController.gameObject.SetActive(false);

        var item = getObjFromProduct(product);
        if (item == null) return;

        EnableEquipment(item.item, equipment ? product.quantity : -1);
    }

    void DisableCharacters()
    {
        List<CharacterName> toRemove = new List<CharacterName>();
        toRemove.Add(CharacterName.ROXY);
        toRemove.Add(CharacterName.LUCA);
        toRemove.Add(CharacterName.PHIL);
        toRemove.Add(CharacterName.SALLY);
        toRemove.Add(CharacterName.TRIPP);
        toRemove.Add(CharacterName.FLORIAN);

        foreach (var c in chosenThisWeek) toRemove.Remove(c);
        foreach (var c in toRemove) DialogueManager.instance.DisableCharacterStory(c);
    }

    ProductObject getObjFromProduct(Product product)
    {
        foreach (var p in productObj) if (p.product == product) return p;

        return null;
    }
    
    public void EnableEquipment(Item newEquipment, int quantity = -1)
    {
        if (unlockedEquipment.Contains(newEquipment)) return;

        unlockedEquipment.Add(newEquipment);
        foreach (var s in allStorage) {
            s.Enable(newEquipment, quantity);
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
