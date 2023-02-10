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

    public UnityEvent OnStoreOpen;
    public UnityEvent OnStoreClose;

    [HideInInspector] public OrderController orderController;
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerWallet wallet;
    [HideInInspector] public NotificationButtonCoordinator notifCoord;
    [HideInInspector] public DayNightController timeScript;
    [HideInInspector] public FollowCamera camScript;
    public bool TEMP_SELECTED_RECIPE;
    public bool TEMP_DELIVERED;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    public void Notify(GameObject obj = null, UnityAction callback = null)
    {
        StartCoroutine(NotifyWhenPossible(obj, callback));
    }
    IEnumerator NotifyWhenPossible(GameObject obj, UnityAction callback)
    {
        while (!notifCoord) yield return new WaitForEndOfFrame();
        notifCoord.Notify(obj, callback);
    }
}
