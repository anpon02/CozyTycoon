using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerMovement : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public int currentSpotInLine;
    [SerializeField] private Vector3 exitPoint;
    [SerializeField] private Table sitSpot;
    private Seeker seek;
    private AIPath path;
    private CustomerOrderController cust;
    private Table currentTable;

    [Header("Animation")]
    [SerializeField] private float idleWaitMin;
    [SerializeField] private float idleWaitMax;
    private SpriteRenderer sprRenderer;
    private Animator anim;
    private bool idleFinished;
    private bool coroutineRunning;
    

    private void Awake() {
        // movemenet
        cust = GetComponentInChildren<CustomerOrderController>();
        seek = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
        currentTable = null;
        currentSpotInLine = -1;

        // animation
        sprRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        idleFinished = false;
        coroutineRunning = false;
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.GetPaused()) return;
        
        // idle anim
        if(GetCurrentTable() == null && !IsMoving()) {
            anim.SetBool("Walking", false);
            if(!coroutineRunning) {
                StartCoroutine("RandomIdle");
            }
        }
        // sitting anim
        else if(GetCurrentTable() != null && !IsMoving()) {
            anim.SetBool("Walking", false);
            anim.SetTrigger("Sit");
            coroutineRunning = false;
        }
        // walking anim
        else if(IsMoving()) {
            anim.SetBool("Walking", true);
            coroutineRunning = false;
        }
        sprRenderer.flipX = MovingRight();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    /*
    * ANIMATION FUNCTIONS
    */
    private IEnumerator RandomIdle() {
        // play idle every randomly
        coroutineRunning = true;
        idleFinished = false;
        anim.SetTrigger("Idle");
        yield return new WaitUntil(CheckIfIdle);
        yield return new WaitForSeconds(Random.Range(idleWaitMin, idleWaitMax));
        coroutineRunning = false;
    }

    private bool CheckIfIdle() {
        return idleFinished;
    }

    public void IdleIsStarting() {
        idleFinished = false;
    }

    public void IdleIsFinished() {
        idleFinished = true;
    }

    /*
    * MOVEMENT FUNCTIONS
    */
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

    // we do not need a list of potential tables anymore
    public bool ComeToEat() { //List<Transform> potentialTables) {
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
