using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCounter : MonoBehaviour
{
    [SerializeField] List<Transform> displays = new List<Transform>();
    [SerializeField] List<ItemCoordinator> heldItems = new List<ItemCoordinator>();
    [SerializeField] Color hoverColor;
    [SerializeField] List<Transform> emptySlots = new List<Transform>();

    private void Update()
    {
        if (heldItems.Count == 0) return;
        ItemCoordinator toRemove = null;
        for (int i = 0; i < heldItems.Count; i++) {
            if (heldItems[i].travellingToChef) {
                toRemove = heldItems[i];
                break;
            }
        }
        if (toRemove) {
            heldItems.Remove(toRemove);
            emptySlots.Add(findSlot(toRemove.gameObject));
        }
    }

    Transform findSlot(GameObject iCoord)
    {
        float bestDist = Vector3.Distance(iCoord.transform.position, displays[0].transform.position);
        Transform best = displays[0];
        foreach (var d in displays) {
            float dist = Vector3.Distance(iCoord.transform.position, d.transform.position);
            if (dist < bestDist) {
                bestDist = dist;
                best = d;
            }
        }
        return best;
    }

    private void Start()
    {
        emptySlots = new List<Transform>(displays);
    }

    private void OnMouseDown()
    {
        ChefController chef = KitchenManager.instance.chef;
        if (!chef) return;

        if (chef.IsHoldingItem() && heldItems.Count < displays.Count) {
            heldItems.Add(chef.GetHeldiCoord());
            chef.ReleaseItem(emptySlots[0].transform.position);
            emptySlots.RemoveAt(0);
        }
    }
    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = hoverColor;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }


}
