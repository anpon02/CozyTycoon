using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerMinigame : MonoBehaviour
{
    [SerializeField] GameObject goalCircle, playerCircle, ringGuide;
    [SerializeField] Vector2 circleCenter;
    [SerializeField] float radius, progressSpeed, mixerSpeed, maxPlayerDist, playerWinDist;
    bool holdingPlayer;


    [SerializeField] WorkstationUICoordinator uiCoord;
    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;     
    }
    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }

    private void Update()
    {
        Vector3 centerPoint = new Vector3(transform.position.x + circleCenter.x, transform.position.y + circleCenter.y, transform.position.z);
        ringGuide.transform.position = centerPoint;

        if (Input.GetMouseButtonUp(0)) holdingPlayer = false;
        PlayerMovementMixer(centerPoint);

        MoveGoalCircle(centerPoint);

        if (Vector3.Distance(playerCircle.transform.position, goalCircle.transform.position) < playerWinDist)
            uiCoord.AddProgress(progressSpeed * Time.deltaTime);
        if (uiCoord.progressSlider.value >= 1) Complete();
    }

    void MoveGoalCircle(Vector2 centerPoint)
    {
        float time = Time.time % Mathf.PI * 2;
        float x = Mathf.Sin(time * mixerSpeed) * radius;
        float y = Mathf.Cos(time * mixerSpeed) * radius;
        goalCircle.transform.position = new Vector3(x + centerPoint.x, y + centerPoint.y, 0);
    }

    void PlayerMovementMixer(Vector2 centerPoint)
    {
        if (!holdingPlayer) {
            playerCircle.transform.position = Vector3.Lerp(playerCircle.transform.position, centerPoint, 0.05f);
            return;
        }

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = playerCircle.transform.position.z;
        var newPos = Vector3.Lerp(playerCircle.transform.position, mouseWorldPos, 0.05f);
        newPos.x = Mathf.Clamp(newPos.x, centerPoint.x - maxPlayerDist, centerPoint.x + maxPlayerDist);
        newPos.y = Mathf.Clamp(newPos.y, centerPoint.y - maxPlayerDist, centerPoint.y + maxPlayerDist);
        playerCircle.transform.position = newPos;
    }

    public void clickDownOnPlayer()
    {
        holdingPlayer = true;
    }
}
