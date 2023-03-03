using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeBookCoordinator : MonoBehaviour
{
    [SerializeField] GameObject bookParent, leftPage, rightPage, recipeEntryPrefab, nextPage, prevPage, recipeButton;
    List<RecipeEntryCoordinator> entryCoords = new List<RecipeEntryCoordinator>();
    [SerializeField] Animator recipeButtonAnim;
    
    [Header("Sounds")]
    [SerializeField] int openbookSound;
    [SerializeField] int closebookSound;
    [SerializeField] int flipPageSound;

    bool mouseOver;
    RecipeManager rMan;
    int currentPageIndex;

    private void Awake()
    {
        recipeButton.SetActive(false);
    }

    private void Start()
    {
        rMan = RecipeManager.instance;
    }

    public void Hover()
    {
        mouseOver = true;
    }

    public void ExitHover()
    {
        mouseOver = false;
    }

    public void ToggleBook()
    {
        if (bookParent.activeInHierarchy) CloseBook();
        else OpenBook();

    }

    public void CloseBook()
    {
        if (!bookParent.activeInHierarchy) return;

        bookParent.SetActive(false);
        PauseManager.instance.numOpenMenus -= 1;
    }

    public void OpenBook()
    {
        if (bookParent.activeInHierarchy) return;

        bookParent.SetActive(true);
        while (rMan.unlockedRecipes.Count > entryCoords.Count) {
            if (!AddNewUnlockedRecipe()) break;
        }
        currentPageIndex = PageWithNewRecipes();


        DisplayCurrentPage();
        AudioManager.instance.PlaySound(openbookSound, gameObject);
        PauseManager.instance.numOpenMenus += 1;
        recipeButton.SetActive(true);
    }

    int PageWithNewRecipes()
    {
        for (int i = 0; i < entryCoords.Count; i++) {
            if (!entryCoords[i].read) return Mathf.FloorToInt(i/6);
        }

        return currentPageIndex;
    }

    bool newRecipes()
    {
        foreach (var e in entryCoords) if (!e.read) return true;
        return false;
    }

    bool AddNewUnlockedRecipe()
    {
        var recipe = GetNewRecipe();
        if (recipe == null) return false;

        var newGo = Instantiate(recipeEntryPrefab, bookParent.transform);
        var newRecipeCoord = newGo.GetComponent<RecipeEntryCoordinator>();
        newRecipeCoord.Init(recipe);
        newGo.SetActive(false);

        entryCoords.Add(newRecipeCoord);
        SortEntryCoords();
        return true;
    }

    void SortEntryCoords()
    {
        var temp = new List<RecipeEntryCoordinator>(entryCoords);
        entryCoords.Clear();
        

        do {
            var item = FindLowestIndexInList(temp);
            if (item == null) break;
            MoveFromList1ToList2(entryCoords, temp, item);
        } while (temp.Count > 0);
    }

    void MoveFromList1ToList2(List<RecipeEntryCoordinator> list1, List<RecipeEntryCoordinator> list2, RecipeEntryCoordinator item)
    {
        list1.Add(item);
        list2.Remove(item);
    }

    RecipeEntryCoordinator FindLowestIndexInList(List<RecipeEntryCoordinator> list)
    {
        if (list.Count == 0) return null;
        var lowest = list[0];
        for (int i = 0; i < list.Count; i++) {
            if (list[i].recipe.index < lowest.recipe.index) lowest = list[i];
        }
        return lowest;
    }
    
    Recipe GetNewRecipe()
    {
        foreach (var rUnlocked in rMan.unlockedRecipes) {
            bool found = false;
            foreach (var rCoord in entryCoords) {
                if (rCoord.recipe == rUnlocked) found = true;
            }
            if (!found) return rUnlocked;
        }
        return null;
    }

    void DisplayCurrentPage()
    {
        foreach (var c in entryCoords) c.gameObject.SetActive(false);

        int index = 6 * currentPageIndex;
        int count = 0;
        Transform parent = leftPage.transform;
        while (entryCoords.Count > index && count < 6) {
            entryCoords[index].transform.parent = parent;
            entryCoords[index].gameObject.SetActive(true);
            count += 1;
            index += 1;
            if (count == 3) parent = rightPage.transform;
        }
        ShowButtons();
    }

    public void PreviousPage()
    {
        currentPageIndex -= 1;
        DisplayCurrentPage();
        ShowButtons();
        AudioManager.instance.PlaySound(flipPageSound, gameObject);
    }

    public void NextPage()
    {
        currentPageIndex += 1;
        DisplayCurrentPage();
        ShowButtons();
        AudioManager.instance.PlaySound(flipPageSound, gameObject);
    }

    void ShowButtons()
    {
        prevPage.SetActive(currentPageIndex > 0);
        nextPage.SetActive((currentPageIndex + 1) * 6 < entryCoords.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseBook();
        recipeButtonAnim.SetBool("wiggle", newRecipes() || rMan.unlockedRecipes.Count > entryCoords.Count); 
    }

    string PrintList(string preface, List<Recipe> list)
    {
        string s = preface;
        foreach (var i in list) s += i.GetResult().GetName() + ", ";
        return s;
    }
}
