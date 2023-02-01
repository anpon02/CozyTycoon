using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    [SerializeField] Gradient backgroundGradient;
    [SerializeField] float timeSpeed = 0.5f;
    [SerializeField, Range(0,1)] float time;
    [SerializeField] TextMeshProUGUI timeDisplay;

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
        UpdateButton();
    }

    private void Update()
    {
        DoEvents();
        TickTime();
        DisplayTime();
    }

    void DoEvents()
    {
        if (time >= openTime && time <= closeTime && closed) Open();
        if (time >= closeTime && !closed) Close();
    }

    void TickTime()
    {
        time += Time.deltaTime * (asleep ? 15 * timeSpeed : timeSpeed) * 0.01f;
        if (time >= 1) time = 0;
        Camera.main.backgroundColor = backgroundGradient.Evaluate(time);
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

        timeDisplay.text = (hour > 9 ? hour : "0" + hour) + ":" + (minute > 9 ? minute : "0" + minute) + suffix;
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
        AudioManager.instance.PlaySound(10, gameObject);
        GameManager.instance.OnStoreOpen.Invoke();
        closed = asleep = false;
        UpdateButton();
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
