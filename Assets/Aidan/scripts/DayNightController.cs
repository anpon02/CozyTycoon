using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    [SerializeField] Gradient backgroundGradient;
    [SerializeField] float timeSpeed = 0.5f;
    [SerializeField, Range(0,1)] float time;
    [SerializeField] TextMeshProUGUI timeDisplay;
    [SerializeField] float wakeUpTime = 0.2f;
    [SerializeField] float earliestCloseTime = 0.6f;
    [SerializeField] float sleepTime;

    [Header("CloseButton")]
    [SerializeField] Image buttonImg;
    [SerializeField] Color openColor;
    [SerializeField] Color closedColor;
    [SerializeField] Color sleepColor;
    bool TEMPCLOSED = true;
    bool TEMPASLEEP = false;
    bool openedToday;

    [Header("Schedule")]
    [SerializeField] float openTime = 0.25f;
    [SerializeField] float closeTime = 0.8f;

    private void Start()
    {
        sleepTime = 1;
        UpdateButton();
    }

    private void Update()
    {
        TickTime();
        DisplayTime();
    }

    void TickTime()
    {
        if (TEMPASLEEP && time >= wakeUpTime && time < sleepTime) {
            TEMPASLEEP = false;
            TEMPCLOSED = true;
            openedToday = false;
            UpdateButton();
        }
        time += Time.deltaTime * (TEMPASLEEP ? 15 * timeSpeed : timeSpeed) * 0.01f;
        if (time >= 1) time = 0;
        Camera.main.backgroundColor = backgroundGradient.Evaluate(time);

        if (TEMPCLOSED && !openedToday && time > openTime) Open();
        if (!TEMPCLOSED && openedToday && time > closeTime) {
            Close();
            buttonImg.GetComponent<Button>().enabled = true;
        }
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

    public void _ToggleStore()
    {
        if (TEMPASLEEP) return;
        if (time < earliestCloseTime && !TEMPCLOSED) return;
        if (time > wakeUpTime || TEMPCLOSED) TEMPCLOSED = !TEMPCLOSED;
        if (!TEMPCLOSED) openedToday = true;
        
        UpdateButton();
    }


    public void ToggleStore()
    {
        if (TEMPASLEEP) return;

        if (TEMPCLOSED && openedToday) Sleep();
        else if (TEMPCLOSED) Open();
        else if (!TEMPCLOSED && time >= earliestCloseTime) Close();
        UpdateButton();
    }

    void Close()
    {
        AudioManager.instance.PlaySound(11, gameObject);
        GameManager.instance.OnStoreClose.Invoke();
        TEMPCLOSED = true;
        UpdateButton();
    }

    void Open()
    {
        AudioManager.instance.PlaySound(10, gameObject);
        GameManager.instance.OnStoreOpen.Invoke();
        TEMPCLOSED = false;
        openedToday = true;
        UpdateButton();
    }

    void Sleep()
    {
        TEMPASLEEP = true;
        sleepTime = time;
    }


    void UpdateButton()
    {
        var text = "asleep...";
        Color col = sleepColor;
        var buttonText = buttonImg.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = TEMPASLEEP ? text : (TEMPCLOSED ? (openedToday ? "go to sleep" : "closed") : "open");
        buttonImg.color = TEMPASLEEP ? col : (TEMPCLOSED ? openColor : closedColor);
    }
}
