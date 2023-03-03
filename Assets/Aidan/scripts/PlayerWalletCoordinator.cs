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
    bool animating;

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

        if (animating || string.Equals(text.text, GameManager.instance.wallet.money.ToString())) return;

        StartCoroutine(ShowThenHide());
    }

    IEnumerator ShowThenHide()
    {
        animating = true;
        
        while(Vector2.Distance(transform.parent.localPosition, exposedPosition) > 0.02f) {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, exposedPosition, 0.025f);
            yield return new WaitForEndOfFrame();
        }

        AudioManager.instance.PlaySound(17, gameObject);
        int walletAmount = GameManager.instance.wallet.money;
        int currentAmount = int.Parse(text.text);
        while (currentAmount != walletAmount) {
            currentAmount += 1 * (walletAmount > currentAmount ? 1 : -1);
            text.text = currentAmount.ToString();
            yield return new WaitForEndOfFrame();
        }

        /*yield return new WaitForSeconds(showTime);


        while (Vector2.Distance(transform.parent.localPosition, hiddenPosition) > 0.01f) {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, hiddenPosition, 0.025f);
            yield return new WaitForEndOfFrame();
        }*/
        animating = false;
    }
}
