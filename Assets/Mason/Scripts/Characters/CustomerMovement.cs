using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private Vector3 exitPoint;
    [SerializeField] private Table sitSpot;

    private Seeker seek;
    private AIPath path;
    private CustomerOrderController cust;
    private Table currentTable;
    private int currentSpotInLine;

    private void Awake() {
        cust = GetComponentInChildren<CustomerOrderController>();
        seek = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
        currentTable = null;
        currentSpotInLine = -1;
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void GetInLine() {
        // if not in line or in a farther line spot
        if(currentSpotInLine > LineManager.instance.GetNextOpenSpot() || currentSpotInLine == -1) {
            // get the next available spot from lineManager
            currentSpotInLine = LineManager.instance.GetNextOpenSpot();
            LineSpot spot = LineManager.instance.GetLineSpots()[currentSpotInLine];
            Vector3 spotCoords = spot.GetPlaceCoordinates();

            // move to spot and claim it
            seek.StartPath(transform.position, spotCoords);
            path.destination = spotCoords;
            spot.SetPlaceIsTaken(true);
            LineManager.instance.UpdateNextOpenSpot();
        }
    }

    public void MoveUpInLine() {
        LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);
        GetInLine();
    }

    public bool ComeToEat(List<Transform> potentialTables) {
        if(!cust.GetHasReceivedFood()) {
            LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);  
            LineManager.instance.UpdateNextOpenSpot();

            /*
            // pick a random available table, move to it, and reserve it
            Transform tableChoice = potentialTables[Random.Range(0, potentialTables.Count -1)];
            seek.StartPath(transform.position, tableChoice.position);
            path.destination = tableChoice.position;
            currentTable = tableChoice.GetComponent<Table>();
            currentTable.SetIsTaken(true);*/
            seek.StartPath(transform.position, sitSpot.transform.position);
            path.destination = sitSpot.transform.position;
            currentTable = sitSpot;
            return true;
        }
        return false;
    }

    public void LeaveRestaurant() {
        if(transform.position != exitPoint) {
            seek.StartPath(transform.position, exitPoint);
            path.destination = exitPoint;
            if(currentTable)
                currentTable.SetIsTaken(false);
            currentTable = null;
        }
    }

    public Table GetCurrentTable() {
        return currentTable;
    }

    public bool IsMoving() {
        return path.remainingDistance > 0.1f && path.remainingDistance != Mathf.Infinity;
    }

    public bool MovingRight() {
        return path.destination.x > transform.position.x;
    }
}
