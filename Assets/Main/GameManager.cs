using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake() { instance = this; }

    public bool assistMode;

    public UnityEvent OnStoreOpen;
    public UnityEvent OnStoreClose;

    [HideInInspector] public OrderController orderController;
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerWallet wallet;
    [HideInInspector] public NotificationButtonCoordinator notifCoord;
    [HideInInspector] public DayNightController timeScript;
    [HideInInspector] public FollowCamera camScript;
    [HideInInspector] public bool TEMP_SELECTED_RECIPE;
    [HideInInspector] public bool TEMP_DELIVERED;
    [HideInInspector] public Notebook notebook;
    [SerializeField] bool notifsPaused = true;
    GameObject loggedNotifObj;
    UnityAction loggedNotifCallback;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    public void PauseNotifs()
    {
        notifsPaused = true;
    }

    public void UnPauseNotifs()
    {
        print("Unpaused!");
        notifsPaused = false;
        Notify(loggedNotifObj, loggedNotifCallback);
        loggedNotifObj = null;
        loggedNotifCallback = null;
    }

    public void Notify(GameObject obj = null, UnityAction callback = null)
    {
        if (obj == null && callback == null) return;
        if (notifsPaused) {
            notifCoord.gameObject.SetActive(false);
            loggedNotifObj = obj;
            loggedNotifCallback = callback;
            return;
        }

        StartCoroutine(NotifyWhenPossible(obj, callback));
    }
    IEnumerator NotifyWhenPossible(GameObject obj, UnityAction callback)
    {
        while (!notifCoord) yield return new WaitForEndOfFrame();
        notifCoord.Notify(obj, callback);
    }
}
