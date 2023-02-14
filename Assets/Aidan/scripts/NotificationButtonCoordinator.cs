using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationButtonCoordinator : MonoBehaviour
{
    GameObject toShow;
    UnityAction callback;
    public UnityEvent OnNotify;

    public void Notify(GameObject _toShow, UnityAction _callback = null)
    {
        callback = _callback;
        toShow = _toShow;
        transform.parent.gameObject.SetActive(true);
    }

    private void Start()
    {
        GameManager.instance.notifCoord = this;
    }

    private void OnEnable()
    {
        if (toShow) AudioManager.instance.PlaySound(15, gameObject);
    }

    public void Hover()
    {
        AudioManager.instance.PlaySound(16, gameObject);
    }

    public void Click()
    {
        AudioManager.instance.PlaySound(8, gameObject);
        transform.parent.gameObject.SetActive(false);

        if (callback != null) callback.Invoke();
        if (toShow) toShow.SetActive(true);
    }
}
