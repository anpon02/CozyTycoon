using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    [SerializeField] private float idleWaitMin;
    [SerializeField] private float idleWaitMax;

    private SpriteRenderer sprRenderer;
    private Animator anim;
    private CustomerMovement movement;

    private bool idleFinished;
    private bool coroutineRunning;

    private void Awake() {
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<CustomerMovement>();
        idleFinished = false;
        coroutineRunning = false;
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.GetPaused()) return;
        
        // idle
        if(movement.GetCurrentTable() == null && !movement.IsMoving()) {
            anim.SetBool("Walking", false);
            if(!coroutineRunning) {
                StartCoroutine("RandomIdle");
            }
        }
        // sitting
        else if(movement.GetCurrentTable() != null && !movement.IsMoving()) {
            anim.SetBool("Walking", false);
            anim.SetTrigger("Sit");
            coroutineRunning = false;
        }
        // walking
        else if(movement.IsMoving()) {
            anim.SetBool("Walking", true);
            coroutineRunning = false;
        }

        sprRenderer.flipX = movement.MovingRight();
    }

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
}
