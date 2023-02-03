using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuCoordinator : MonoBehaviour
{
    [SerializeField] GameObject mainParent;
    [SerializeField] List<GameObject> pages = new List<GameObject>();
    [SerializeField] GameObject nextPageButton;
    [SerializeField] GameObject prevPageButton;
    int currentPageIndex = 0;

    public void Toggle()
    {
        mainParent.SetActive(!mainParent.activeInHierarchy);
    }

    public void NextPage()
    {
        updateButtons();
        pages[currentPageIndex].SetActive(false);
        if (currentPageIndex < pages.Count - 1) currentPageIndex += 1;
        pages[currentPageIndex].SetActive(true);
        updateButtons();
    }

    public void PreviousPage()
    {
        print("hi");
        updateButtons();
        pages[currentPageIndex].SetActive(false);
        if (currentPageIndex > 0) currentPageIndex -= 1;
        pages[currentPageIndex].SetActive(true);
        updateButtons();
    }

    void updateButtons()
    {
        prevPageButton.SetActive(true);
        nextPageButton.SetActive(true);

        if (currentPageIndex >= pages.Count - 1) {
            nextPageButton.SetActive(false);
        }
        else if (currentPageIndex == 0) {
            prevPageButton.SetActive(false);
        }
    }
}
