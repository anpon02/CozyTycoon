using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class OrderUICoordinator : MonoBehaviour
{
    [SerializeField] GameObject listElementPrefab, OrderParent;
    [SerializeField] Transform listParent;
    List<OrderListItemCoordinator> listItem = new List<OrderListItemCoordinator>();
    
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
        listItem.Add(listScript);
    }

    public void completeItem(CharacterName character)
    {
        var list = FindListItem(character);
        if (list.Count == 0) return;

        foreach (var l in list) l.MarkComplete();
    }

    public void RemoveItem(CharacterName character)
    {
        var toRemove = FindListItem(character);
        if (toRemove.Count == 0) return;

        foreach (var r in toRemove) 
        for (int i = 0; i < toRemove.Count; i++) {
            listItem.Remove(toRemove[i]);
                Destroy(toRemove[i].gameObject);
        }

        if (listItem.Count == 0) OrderParent.SetActive(false);
    }

    List<OrderListItemCoordinator> FindListItem(CharacterName character)
    {
        var list = new List<OrderListItemCoordinator>();
        for (int i = 0; i < listItem.Count; i++) {
            if (listItem[i].character == character) list.Add(listItem[i]);
        }
        return list;
    }
}
