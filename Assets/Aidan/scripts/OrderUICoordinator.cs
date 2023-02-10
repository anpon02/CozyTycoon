using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class OrderUICoordinator : MonoBehaviour
{
    [SerializeField] GameObject listElementPrefab, OrderParent;
    [SerializeField] Transform listParent;
    List<OrderListItemCoordinator> listItems = new List<OrderListItemCoordinator>();
    
    public void AddNew(string itemName, float patience, CharacterName character)
    {
        StartCoroutine(WaitThenAdd(itemName, patience, character, OrderParent.activeInHierarchy ? 0 : 0.5f)); 
    }

    IEnumerator WaitThenAdd(string itemName, float patience, CharacterName character, float waitTime)
    {
        OrderParent.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        AudioManager.instance.PlaySound(7, gameObject);
        var newListItem = Instantiate(listElementPrefab, listParent);
        var listScript = newListItem.GetComponent<OrderListItemCoordinator>();
        listScript.Init(itemName, patience, character);
        listItems.Add(listScript);
    }

    public void completeItem(CharacterName character)
    {
        var listItem = FindListItem(character);
        if (listItem == null) return;

        listItem.MarkComplete();
    }

    public void RemoveItem(CharacterName character)
    {
        var listItem = FindListItem(character);
        if (listItem == null) return;

        Destroy(listItem.gameObject);
    }

    OrderListItemCoordinator FindListItem(CharacterName character)
    {
        for (int i = 0; i < listItems.Count; i++) {
            if (listItems[i].character == character) return listItems[i];
        }
        return null;
    }
}
