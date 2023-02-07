using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class PlayerWalletCoordinator : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] bool setExposed;
    [SerializeField] Vector3 exposedPosition;
    [SerializeField] bool setHidden;
    [SerializeField] Vector3 hiddenPosition;
    [SerializeField] float showTime = 2.5f;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (setExposed) {
            setExposed = false;
            exposedPosition = transform.parent.localPosition;
        }
        if (setHidden) {
            setHidden = false;
            hiddenPosition = transform.parent.localPosition;
        }

        if (!Application.isPlaying) return;

        if (string.Equals(text.text, GameManager.instance.wallet.money.ToString())) return;

        text.text = GameManager.instance.wallet.money.ToString();
        StartCoroutine(ShowThenHide());
    }

    IEnumerator ShowThenHide()
    {
        while(Vector2.Distance(transform.parent.localPosition, exposedPosition) > 0.01f) {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, exposedPosition, 0.025f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(showTime);


        while (Vector2.Distance(transform.parent.localPosition, hiddenPosition) > 0.01f) {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, hiddenPosition, 0.025f);
            yield return new WaitForEndOfFrame();
        }
    }
}
