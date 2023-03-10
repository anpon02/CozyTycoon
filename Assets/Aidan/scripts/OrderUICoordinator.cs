using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class OrderUICoordinator : MonoBehaviour
{
    [SerializeField] GameObject listElementPrefab, OrderParent;
    [SerializeField] Transform listParent;
    [HideInInspector] public List<OrderEntryCoordinator> orderEntryCoords = new List<OrderEntryCoordinator>();

    private void Update()
    {
        if (DialogueManager.instance == null) return;
        DialogueManager.instance.SetTextSkip(orderEntryCoords.Count == 0);
    }

    public void AddNew(string itemName, string sideName, CharacterName character)
    {
        AudioManager.instance.PlaySound(7, gameObject);
        var newListItem = Instantiate(listElementPrefab, listParent);
        var listScript = newListItem.GetComponent<OrderEntryCoordinator>();
        listScript.uiCoord = this;
        listScript.Init(itemName, sideName, DialogueManager.instance.GetSpeakerData(character).lilPortrait, character);

    }

    public void HideAllOrders()
    {
        foreach (var o in orderEntryCoords) o.Hide();
    }

    public void completeItem(CharacterName character)
    {
        var list = FindListItem(character);
        if (list.Count == 0) return;
    }

    public void RemoveItem(CharacterName character)
    {
        var toRemove = FindListItem(character);
        if (toRemove.Count == 0) return;

        foreach (var r in toRemove) 
        for (int i = 0; i < toRemove.Count; i++) {
            orderEntryCoords.Remove(toRemove[i]);
            Destroy(toRemove[i].gameObject);
        }
    }

    List<OrderEntryCoordinator> FindListItem(CharacterName character)
    {
        var list = new List<OrderEntryCoordinator>();

        for (int i = 0; i < orderEntryCoords.Count; i++) {
            if (orderEntryCoords[i].character == character) list.Add(orderEntryCoords[i]);
        }
        return list;
    }
}
