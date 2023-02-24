using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour {
    public enum Day {Mon, Tue, Wed, Thur, Fri, Sat, Sun };
    Day currentDay;

    [SerializeField] Gradient backgroundGradient;
    [SerializeField] float timeSpeed = 0.5f;
    [Range(0,1)] public float time;
    TextMeshProUGUI timeDisplay;
    [SerializeField] GameObject wheel;
    [SerializeField] float wheelOffset;
    [HideInInspector] public bool paused;
    [SerializeField] TextMeshProUGUI date;
    public int day;

    [Header("CloseButton")]
    [SerializeField] Image buttonImg;
    [SerializeField] Color openColor;
    [SerializeField] Color closedColor;
    [SerializeField] Color sleepColor;
   
    [Header("Schedule")]
    [SerializeField] float openTime = 0.25f;
    [SerializeField] float closeTime = 0.8f;
    bool closed = true;
    bool asleep = false;

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
        NextDay();
        time = openTime - 0.01f;
        if (!closed) Close();
        GoToSleep();
        timeSpeed = 1;
    }

    private void Update()
    {
        DoEvents();
        TickTime();
        DisplayTime();
        SpinWheel();
    }

    void SpinWheel()
    {
        wheel.transform.localEulerAngles = new Vector3(0, 0, (time * 360) + wheelOffset);
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

    void DisplayTime()
    {
        float longTime = time * 2400;
        float hour = Mathf.FloorToInt(longTime / 100);
        float minute = longTime - (hour*100);
        minute /= 100;
        minute = Mathf.RoundToInt(minute * 60);
        string suffix = hour > 12 ? " pm" : " am";
        hour = hour > 12 ? hour - 12 : hour;
        if (hour == 0) hour = 12;
        minute -= minute % 5;
        if (minute == 60) minute = 55;
        if (timeDisplay) timeDisplay.text = (hour > 9 ? hour : "0" + hour) + ":" + (minute > 9 ? minute : "0" + minute) + suffix;
    }

    public void GoToSleep()
    {
        if (closed && time > closeTime) asleep = true;
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
        //print("STORE OPEN");
        AudioManager.instance.PlaySound(10, gameObject);
        GameManager.instance.OnStoreOpen.Invoke();
        closed = asleep = false;
        UpdateButton();
        SetTimeScaleBasedOnPatience();
    }

    void SetTimeScaleBasedOnPatience() {
        float totalTime = CustomerManager.instance.todaysCombinedPatience;
        totalTime *= 3f;

        float _timeScale = 1;

        float dayLength = closeTime - time;

        _timeScale = (dayLength / totalTime);

        //print("total time: " + totalTime + ", timeScale: " + _timeScale + ", dayLength: " + dayLength);
        timeSpeed = _timeScale;
    }

    void UpdateButton()
    {
        var text = "asleep...";
        Color col = sleepColor;
        var buttonText = buttonImg.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = asleep ? text : (closed ? (time > closeTime ? "go to sleep" : "closed") : "open");
        buttonImg.color = asleep ? col : (closed ? openColor : closedColor);
    }
}
