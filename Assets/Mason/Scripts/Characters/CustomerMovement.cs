using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerMovement : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public int currentSpotInLine;
    [HideInInspector] public bool inRestaurant;
    [SerializeField] private Vector3 exitPoint;
    [SerializeField] private Table sitSpot;
    [SerializeField] private float minSpeed = 3.0f;
    [SerializeField] private float maxSpeed = 5.0f;
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
        if(PauseManager.instance && PauseManager.instance.paused) return;

        // idle anim
        if(InLine()) {
            anim.SetBool("Walking", false);
            if(!coroutineRunning) {
                StartCoroutine("RandomIdle");
            }
        }
        // sitting anim
        else if(AtTable()) {
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

        // move faster when closer to exit point
        ChangeSpeed();
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
    private void ChangeSpeed() {
        // this is kinda jankily done, I'm gonna make this more smooth later
        if(Vector2.Distance(transform.position, exitPoint) < 15.0f)
            path.maxSpeed = maxSpeed;
        else
            path.maxSpeed = minSpeed;
    }

    private IEnumerator AtExitPoint() {
        yield return new WaitUntil(() => Vector2.Distance(transform.position, exitPoint) < 0.15f);
        transform.position = exitPoint;
    }

    public void GetInLine() {
        GetComponent<CustomerCoordinator>().storyFinished = false;
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
            inRestaurant = true;
        }
    }

    public void MoveUpInLine() {
        LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);
        GetInLine();
    }

    public void ComeToEat() {
        if(!cust.GetHasReceivedFood()) {
            LineManager.instance.GetLineSpots()[currentSpotInLine].SetPlaceIsTaken(false);  
            LineManager.instance.UpdateNextOpenSpot();

            seek.StartPath(transform.position, sitSpot.transform.position);
            path.destination = sitSpot.transform.position;
            currentTable = sitSpot;
        }
    }

    public void LeaveRestaurant() {
        if(transform.position != exitPoint) {
            seek.StartPath(transform.position, exitPoint);
            path.destination = exitPoint;
            if(currentTable)
                currentTable.SetIsTaken(false);
            currentTable = null;
            inRestaurant = false;
        }
    }

    public Table GetCurrentTable() {
        return currentTable;
    }

    public bool IsMoving() {
        return path.remainingDistance > 0.15f && path.remainingDistance != Mathf.Infinity;
    }

    public bool MovingRight() {
        return path.destination.x > transform.position.x;
    }

    public bool InLine() {
        return GetCurrentTable() == null && !IsMoving() && Vector2.Distance(transform.position,exitPoint) > 1;
    }

    public bool AtTable() {
        return GetCurrentTable() != null && !IsMoving();
    }
}
