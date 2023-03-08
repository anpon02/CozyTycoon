using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour {
    public enum Day {Mon, Tue, Wed, Thur, Fri, Sat, Sun };
    Day currentDay;

    [SerializeField] Gradient backgroundGradient;
    [SerializeField] float timeSpeed = 0.5f, fastSpeed = 0.5f;
    [Range(0,1)] public float time;
    [HideInInspector] public bool paused;
    [SerializeField] TextMeshProUGUI date;
    public int day;

    [Header("CloseButton")]
    [SerializeField] Image buttonImg;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closedSprite;
   
    [Header("Schedule")]
    [SerializeField] float openTime = 0.25f;
    [SerializeField] float closeTime = 0.8f;
    bool closed = true;
    bool asleep = false;

    [Header("nightTime")]
    [SerializeField] GameObject nightWipe;
    [SerializeField] GameObject calendar;

    private void Start()
    {
        GameManager.instance.timeScript = this;
        UpdateButton();
        DisplayDate();   
    }

    void DisplayDate()
    {
        string suffix = "th";
        if (day == 0) suffix = "st";
        if (day == 1) suffix = "nd";
        if (day == 2) suffix = "rd";
        date.text = currentDay.ToString() + ", " + (day + 1) + "<voffset=0.46em><size=60%>" + suffix;
    }

    public void PauseTime()
    {
        paused = true;
    }

    public void UnpauseTime()
    {
        paused = false;
    }

    public void LastCustomerLeave()
    {
        time = closeTime + 0.01f;
        if (!closed) Close();
        GoToSleep();
        timeSpeed = fastSpeed;
    }

    private void Update()
    {
        DoEvents();
        TickTime();
    }

    void DoEvents()
    {
        if (time >= openTime && time <= closeTime && closed) Open();
        if (time >= closeTime && !closed) Close();
    }

    void TickTime()
    {
        if (paused) return;

        time += Time.deltaTime * (asleep ? 15 * timeSpeed : timeSpeed);
        if (time >= 1) NextDay();
        Camera.main.backgroundColor = backgroundGradient.Evaluate(time);
    }

    void NextDay()
    {
        time = 0; 
        day += 1;
        currentDay = (Day) (day % 7);
        DisplayDate();
    }

    public void GoToSleep()
    {
        if (closed && time > closeTime) asleep = true;
        if (!asleep) return;

        nightWipe.SetActive(true);
        StartCoroutine(OpenCalendar());
    }

    IEnumerator OpenCalendar()
    {
        yield return new WaitForSeconds(1.2f);
        calendar.gameObject.SetActive(true);
    }

    void Close()
    {
        AudioManager.instance.PlaySound(11, gameObject);
        GameManager.instance.OnStoreClose.Invoke();
        closed = true;
        UpdateButton();
    }

    void Open()
    {
        AudioManager.instance.PlaySound(10, gameObject);
        GameManager.instance.OnStoreOpen.Invoke();
        closed = asleep = false;
        UpdateButton();
        timeSpeed = 0;
    }


    void UpdateButton()
    {
        buttonImg.sprite = closed ? closedSprite : openSprite;
    }
}
