using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteAlways]
public class WorkstationUICoordinator : MonoBehaviour
{
    [SerializeField] WorkspaceController ws;
    

    [Header("Contents")]
    [SerializeField] GameObject contentsParent;
    [SerializeField] TextMeshProUGUI content1;
    [SerializeField] TextMeshProUGUI content2;
    [SerializeField] TextMeshProUGUI content3;
    [SerializeField] TextMeshProUGUI content4;
    [SerializeField] TextMeshProUGUI content5;

    [Header("Radial")]
    [SerializeField] GameObject radialParent;
    [SerializeField] float minHoldTime = 0.4f;
    [SerializeField] RadialCoordinator radial1;
    [SerializeField] RadialCoordinator radial2;
    [SerializeField] RadialCoordinator radial3;
    float holdTime;
    Vector3 radialStartScale;

    [Header("ProgressBar")]
    [SerializeField] GameObject progressBarParent;
    [SerializeField] Slider progressSlider;

    [Header("Sounds")]
    [SerializeField] int knifeChopSound;
    [SerializeField] int knifeMissSound, progressSound, completeSound, panFlipPrompt, failSound;

    [Header("Knife Minigame")]
    [SerializeField] bool knifeActive;
    [SerializeField] GameObject knifeParent;
    [SerializeField] GameObject knife, leftEdge, rightEdge, central, knifeTarget;
    [SerializeField] float knifeSpeed = 1, knifePenalty = 2;
    [SerializeField] int requiredChops = 4;
    bool movingLeft;

    [Header("Pan Minigame")]
    [SerializeField] bool panActive;
    [SerializeField] GameObject panParent, flipButton, buttonBounds;
    [SerializeField] Vector2 flipTimeGap = new Vector2(0.1f, 0.6f);
    [SerializeField] float panProgressSpeed = 0.1f, panPenalty = 0.2f, flipFloatMax = 0.6f;
    float nextFlipTime = 0.2f;
    float flipFailTime = 0.5f;

    [Header("Mixer Minigame")]
    [SerializeField] bool mixerActive;
    [SerializeField] GameObject mixerParent, goalCircle, playerCircle, ringGuide;
    [SerializeField] Vector2 circleCenter;
    [SerializeField] float radius, mixerSpeed, maxPlayerDist, playerWinDist, mixerProgressSpeed;
    bool holdingPlayer;
    

    public void StartMinigame(Minigame minigame)
    {
        KitchenManager.instance.minigameStarted = true;
        flipButton.SetActive(false);
        GameManager.instance.camScript.followMouse = false;

        if (minigame == Minigame.KNIFE) knifeActive = true;
        if (minigame == Minigame.PAN) panActive = true;
        if (minigame == Minigame.MIXER) mixerActive = true;
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        radialStartScale = radialParent.transform.localScale;
    }

    private void Update()
    {
        progressBarParent.SetActive(false);
        panParent.SetActive(false);
        knifeParent.SetActive(false);
        mixerParent.SetActive(false);

        if (!Application.isPlaying) return;

        DisplayContents();
        Radial();
        DoMinigame();
        DisplayProgressSlider();
    }

    void DisplayProgressSlider()
    {
        progressBarParent.SetActive(IsMinigameActive());

        if (progressSlider.value >= 1) CompleteRecipe();
    }

    bool IsMinigameActive()
    {
        return panActive || knifeActive || mixerActive;
    }

    void StopAllMinigames()
    {
        panActive = knifeActive = mixerActive = false;
    }

    void CompleteRecipe()
    {

        progressSlider.value = 0;
        StopAllMinigames();
        ws.CompleteRecipe();
        KitchenManager.instance.minigameCompleted = true;
    }

    void DoMinigame()
    {
        if (knifeActive) KnifeMinigame();
        else knifeParent.SetActive(false);

        if (panActive) PanMinigame();
        else panParent.SetActive(false);

        if (mixerActive) MixerMinigame();
        else mixerParent.SetActive(false);
    }

    void MixerMinigame()
    {
        mixerParent.SetActive(true);
        Vector3 centerPoint = new Vector3(transform.position.x + circleCenter.x, transform.position.y + circleCenter.y, transform.position.z);
        ringGuide.transform.position = centerPoint;

        if (Input.GetMouseButtonUp(0)) holdingPlayer = false;
        PlayerMovementMixer(centerPoint);

        MoveGoalCircle(centerPoint);

        if (Vector3.Distance(playerCircle.transform.position, goalCircle.transform.position) < playerWinDist)
            progressSlider.value += mixerProgressSpeed * Time.deltaTime;
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

    void PanMinigame()
    {
        panParent.SetActive(true);
        nextFlipTime -= Time.deltaTime;
        progressSlider.value += panProgressSpeed * Time.deltaTime;
        if (nextFlipTime <= 0 && !flipButton.activeInHierarchy) DisplayFlipButton();
        if (flipButton.activeInHierarchy) {
            flipFailTime -= Time.deltaTime;
            if (flipFailTime <= 0) PanFail();
        }
    }

    void DisplayFlipButton() {
        AudioManager.instance.PlaySound(panFlipPrompt, gameObject);
        flipButton.SetActive(true);

        Vector3 pos = flipButton.transform.position;
        var rect = buttonBounds.GetComponent<RectTransform>().rect;
        Vector2 xy = new Vector2(buttonBounds.transform.localPosition.x, buttonBounds.transform.localPosition.y);
        pos.x = Random.Range(xy.x - rect.width / 2, xy.x + rect.width / 2);
        pos.y = Random.Range(xy.y - rect.height / 2, xy.y + rect.height / 2);
        flipButton.transform.localPosition = pos;

        flipFailTime = flipFloatMax;
    }

    void PanFail() {
        AudioManager.instance.PlaySound(failSound, gameObject);
        progressSlider.value -= panPenalty;
        ResetFlip();
    }

    public void Flip() {
        AudioManager.instance.PlaySound(progressSound, gameObject);
        ResetFlip();
    }

    void ResetFlip() {
        flipButton.SetActive(false);
        nextFlipTime = Random.Range(flipTimeGap.x, flipTimeGap.y);
    }

    void KnifeMinigame()
    {
        knifeParent.SetActive(true);
        GameObject targetEdge = movingLeft ? leftEdge : rightEdge;
        float speed = Time.deltaTime * (movingLeft ? -knifeSpeed : knifeSpeed);
        knife.transform.Translate(Vector3.right * speed, Space.World);
        if (knife.transform.position.x > rightEdge.transform.position.x) movingLeft = true;
        if (knife.transform.position.x < leftEdge.transform.position.x) movingLeft = false;
        
    }

    public void Chop()
    {
        if (knifeSpeed == 0) return;

        if (KnifeInCentral()) {
            
            progressSlider.value += 1.0f / requiredChops;
            StartCoroutine(ChopAnim());
            return;
        }
        StartCoroutine(StopThenStartKnife());
    }

    bool KnifeInCentral()
    {
        float kPos = knife.transform.localPosition.x;
        float cPos = central.transform.localPosition.x;
        float cWidth = central.GetComponent<RectTransform>().rect.width;
        if (kPos <  cPos + cWidth/2 && kPos > cPos - cWidth/2) {
            return true;
        }
        return false;
    }

    IEnumerator ChopAnim()
    {
        float originalSpeed = knifeSpeed;
        Vector3 originalPos = knife.transform.position;
        knifeTarget.transform.position = new Vector3(knife.transform.position.x, knifeTarget.transform.position.y, knifeTarget.transform.position.z);
        knifeSpeed = 0;
        float smoothness = 0.2f;
        AudioManager.instance.PlaySound(knifeChopSound, gameObject);
        AudioManager.instance.PlaySound(progressSlider.value >= 1 ? completeSound : progressSound);

        while (Vector3.Distance(knife.transform.position, knifeTarget.transform.position) > 0.01f) {
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeTarget.transform.position, smoothness);
            yield return new WaitForEndOfFrame();
        }
        
        while (Vector3.Distance(knife.transform.position, originalPos) > 0.01f) {
            knife.transform.position = Vector3.Lerp(knife.transform.position, originalPos, smoothness);
            yield return new WaitForEndOfFrame();
        }
        knifeSpeed = originalSpeed;
    }

    IEnumerator StopThenStartKnife()
    {
        float originalSpeed = knifeSpeed;
        knifeSpeed = 0;
        var knifeImg = knife.GetComponent<Image>();
        knifeImg.color = Color.red;

        AudioManager.instance.PlaySound(failSound, gameObject);
        yield return new WaitForSeconds(knifePenalty);

        knifeImg.color = Color.white;
        knifeSpeed = originalSpeed;
    }

    void Radial()
    {
        if (IsMinigameActive()) return;

        if (contentsParent.activeInHierarchy && Input.GetMouseButton(0)) holdTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(0)) {
            HideRadial();
        }
        if (holdTime >= minHoldTime) ShowRadial();
    }

    void HideRadial()
    {
        holdTime = 0;
        radialParent.transform.localScale = Vector3.zero;
        radialParent.SetActive(false);
        if (!string.IsNullOrEmpty(ws.chosenRecipe)) ws.StartCooking();
    }

    void ShowRadial()
    {
        var list = ws.GetValidRecipeResults();
        radial2.init(list.Count > 0 ? list[0] : null);
        radial1.init(list.Count > 1 ? list[1] : null);
        radial3.init(list.Count > 2 ? list[2] : null);

        radialParent.SetActive(true);
        radialParent.transform.localScale = Vector3.Lerp(radialParent.transform.localScale, radialStartScale, 0.1f);
    }    

    void DisplayContents()
    {
        contentsParent.SetActive(!IsMinigameActive() && !radialParent.activeInHierarchy && ws == KitchenManager.instance.hoveredController);
        content2.text = content3.text = content4.text = content5.text = "";

        content1.text = ws.workSpaceType.ToString();
        var list = ws.GetItemList();
        if (list.Count > 0) content2.text = list[0].GetName().ToString();
        if (list.Count > 1) content3.text = list[1].GetName().ToString();
        if (list.Count > 2) content4.text = list[2].GetName().ToString();
        if (list.Count > 3) content5.text = list[3].GetName().ToString();
    }
}
