using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeBookCoordinator : MonoBehaviour
{
    [SerializeField] GameObject bookParent, leftPage, rightPage, recipeEntryPrefab, nextPage, prevPage;
    List<RecipeEntryCoordinator> entryCoords = new List<RecipeEntryCoordinator>();
    
    [Header("Sounds")]
    [SerializeField] int openbookSound;
    [SerializeField] int closebookSound;
    [SerializeField] int flipPageSound;

    bool mouseOver;
    RecipeManager rMan;
    int currentPageIndex;

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
        DisplayCurrentPage();
        AudioManager.instance.PlaySound(openbookSound, gameObject);
        PauseManager.instance.numOpenMenus += 1;
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
        return true;
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
        if (rMan.unlockedRecipes.Count > entryCoords.Count) GameManager.instance.Notify(callback: OpenBook);
    }
}
