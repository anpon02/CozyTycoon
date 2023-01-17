using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorkspaceCoordinator))]
public class WorkspaceController : MonoBehaviour
{
    WorkspaceCoordinator coord;

    private void Start()
    {
        coord = GetComponent<WorkspaceCoordinator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        if (item != null) CatchItem(item);
    }
    void CatchItem(Item item)
    {
        item.Hide();
        coord.AddItem(item);
    }
}
