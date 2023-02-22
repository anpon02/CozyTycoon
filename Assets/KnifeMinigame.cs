using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeMinigame : MonoBehaviour
{
    [SerializeField] GameObject knife, leftEdge, rightEdge, central, knifeTarget;
    [SerializeField] float knifeSpeed = 1, knifePenalty = 2, chopLerpFactor = 0.2f;
    [SerializeField] int requiredChops = 4;
    bool movingLeft;
    [SerializeField] int knifeChopSound, knifeMissSound;

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
        MoveKnife();
    }

    void MoveKnife()
    {
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

            uiCoord.AddProgress(1.0f / requiredChops);
            if (uiCoord.progressSlider.value >= 1) Complete();
            else StartCoroutine(ChopAnim());
            return;
        }
        StartCoroutine(StopThenStartKnife());
    }
    IEnumerator ChopAnim()
    {
        float originalSpeed = knifeSpeed;
        Vector3 originalPos = knife.transform.position;
        var target = new Vector3(knife.transform.position.x, knifeTarget.transform.position.y, knifeTarget.transform.position.z);
        knifeTarget.transform.position = target;
        knifeSpeed = 0;
        AudioManager.instance.PlaySound(knifeChopSound, gameObject);

        while (Vector3.Distance(knife.transform.position, knifeTarget.transform.position) > 0.01f) {
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeTarget.transform.position, chopLerpFactor);
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Distance(knife.transform.position, originalPos) > 0.01f) {
            knife.transform.position = Vector3.Lerp(knife.transform.position, originalPos, chopLerpFactor);
            yield return new WaitForFixedUpdate();
        }
        knifeSpeed = originalSpeed;
    }

    bool KnifeInCentral()
    {
        float kPos = knife.transform.localPosition.x;
        float cPos = central.transform.localPosition.x;
        float cWidth = central.GetComponent<RectTransform>().rect.width;
        if (kPos < cPos + cWidth / 2 && kPos > cPos - cWidth / 2) {
            return true;
        }
        return false;
    }

    IEnumerator StopThenStartKnife()
    {
        float originalSpeed = knifeSpeed;
        knifeSpeed = 0;
        var knifeImg = knife.GetComponent<Image>();
        knifeImg.color = Color.red;

        uiCoord.PlayFailSound();
        yield return new WaitForSeconds(knifePenalty);

        knifeImg.color = Color.white;
        knifeSpeed = originalSpeed;
    }

}
