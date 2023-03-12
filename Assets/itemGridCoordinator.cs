using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemGridCoordinator : MonoBehaviour
{
    [SerializeField] List<GameObject> parents;
    [SerializeField] ItemStorage itemStorage;
    [SerializeField] GameObject chosenItem;
    [SerializeField] ItemSelectorCoordinator buttonCoord;
    public int Selected = -1;

    public void SelectInt(int num)
    {
        Selected = num;
    }

    public void DisplayGrid(List<ItemStorage.ItemData> items)
    {
        int num = items.Count;
        gameObject.SetActive(true);
        Selected = -1;

        DisplayItems(items);
    }

    void DisplayItems(List<ItemStorage.ItemData> items)
    {
        buttonCoord.buttonVisible = false;
        chosenItem.SetActive(false);
        ShowAndHideItems(items.Count);

        for (int i = 0; i < items.Count; i++) {
            int childIndex = i % 2;
            int parentIndex = Mathf.FloorToInt(i / 2);
            SetupItem(parents[parentIndex].transform.GetChild(childIndex), items[i], i);
        }
    }

    void ShowAndHideItems(int num)
    {
        foreach (var p in parents) { p.SetActive(false); p.transform.GetChild(1).gameObject.SetActive(true); }
        if (num > 0) parents[0].SetActive(true);
        if (num > 2) parents[1].SetActive(true);
        if (num > 4) parents[2].SetActive(true);
        if (num > 6) parents[3].SetActive(true);

        int childIndex = num % 2;
        int parentIndex = (int)Mathf.Floor(num / 2);
        if (childIndex == 0 || parentIndex >= parents.Count) return;

        parents[parentIndex].transform.GetChild(1).gameObject.SetActive(false);
    }

    void SetupItem(Transform obj, ItemStorage.ItemData data, int itemIndex)
    {
        if (!obj.gameObject.activeInHierarchy) return;

        obj.GetComponent<itemStorageSelectedButton>().itemIndex = itemIndex;
        obj.GetChild(0).GetComponent<Image>().sprite = data.item.GetSprite();
        obj.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.item.GetName();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            if (Selected != -1)SelectHovered();
            Hide();
        }
    }
    void SelectHovered()
    {
        itemStorage.SelectItem(Selected);
    }

    private void OnDisable()
    {
        Hide();
    }

    void Hide()
    {
        buttonCoord.buttonVisible = true;
        chosenItem.SetActive(true);
        gameObject.SetActive(false);
    }
}
