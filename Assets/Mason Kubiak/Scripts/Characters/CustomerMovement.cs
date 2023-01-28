using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 exitPoint;

    private Seeker seek;
    private CustomerOrderController cust;
    private Table currentTable;

    private int currentSpotInLine;

    private void Awake() {
        cust = GetComponentInChildren<CustomerOrderController>();
        seek = GetComponent<Seeker>();
    }

    public void GetInLine() {
        // get the next available spot from lineManager
        currentSpotInLine = LineManager.instance.GetNextOpenSpot();
        LineSpot spot = LineManager.instance.GetLineSpots()[currentSpotInLine];
        Vector3 spotCoords = spot.GetPlaceCoordinates();

        // move to spot and claim it
        seek.StartPath(transform.position, spotCoords);
        spot.SetPlaceIsTaken(true);
        LineManager.instance.UpdateNextOpenSpot();
    }

    public void MoveUpInLine() {
        LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);
        GetInLine();
    }

    public bool ComeToEat(List<Transform> potentialTables) {
        if(!cust.GetHasReceivedFood()) {
            LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);
            LineManager.instance.UpdateNextOpenSpot();

            // pick a random available table, move to it, and reserve it
            Transform tableChoice = potentialTables[Random.Range(0, potentialTables.Count -1)];
            seek.StartPath(transform.position, tableChoice.position);
            currentTable = tableChoice.GetComponent<Table>();
            currentTable.SetIsTaken(true);
            return true;
        }
        return false;
    }

    public void LeaveRestaurant() {
        if(transform.position != exitPoint) {
            seek.StartPath(transform.position, exitPoint);
            currentTable.SetIsTaken(false);
        }
    }
}
