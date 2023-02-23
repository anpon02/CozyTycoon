using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanMinigame : MonoBehaviour
{
    [SerializeField] GameObject flipButton, buttonBounds;
    [SerializeField] Vector2 flipTimeGap = new Vector2(0.1f, 0.6f);
    [SerializeField] float panProgressSpeed = 0.1f, flipFloatMax = 0.6f, panStunTime = 0.75f;
    float nextFlipTime = 0.2f, flipFailTime = 0.5f;
    [SerializeField] AudioSource panloopingSource;
    [SerializeField] AudioSource oneShotsSource;
    [SerializeField] int panFlipPrompt;
    float originalSpeed;

    [SerializeField] WorkstationUICoordinator uiCoord;
    private void OnEnable()
    {
        AudioManager.instance.PlaySound(4, panloopingSource);
        originalSpeed = panProgressSpeed;
        uiCoord.ongoingMinigames += 1;
        flipButton.SetActive(false);
    }
    void Complete()
    {
        panloopingSource.Stop();
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }

    public void Flip()
    {
        AudioManager.instance.PlaySound(uiCoord.progressSound, gameObject);
        StopAllCoroutines();
        panProgressSpeed = originalSpeed;
        ResetFlip();
    }

    private void Update()
    {
        nextFlipTime -= Time.deltaTime;
        uiCoord.AddProgress( panProgressSpeed * Time.deltaTime);
        if (uiCoord.progressSlider.value >= 1) { Complete(); return; }

        if (nextFlipTime <= 0 && !flipButton.activeInHierarchy) DisplayFlipButton();
        if (flipButton.activeInHierarchy) {
            flipFailTime -= Time.deltaTime;
            if (flipFailTime <= 0) PanFail();
        }
    }

    void DisplayFlipButton()
    {
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

    void PanFail()
    {
        uiCoord.PlayFailSound();
        StartCoroutine(WaitThenResumePan());
        ResetFlip();
    }

    IEnumerator WaitThenResumePan()
    {
        panProgressSpeed = 0;
        yield return new WaitForSeconds(panStunTime);
        panProgressSpeed = originalSpeed;
    }

    void ResetFlip()
    {
        flipButton.SetActive(false);
        nextFlipTime = Random.Range(flipTimeGap.x, flipTimeGap.y);
    }


}
