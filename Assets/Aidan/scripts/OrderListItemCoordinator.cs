using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderListItemCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Color completeCol;
    [SerializeField] Image timer;
    [SerializeField] bool TESTBOOL;
    [SerializeField] GameObject strikeThrough;
    public float patience;
    float timeLeft;
    string itemName;

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timer.fillAmount = Mathf.Clamp01(timeLeft / patience);

        if (TESTBOOL) {
            TESTBOOL = false;
            MarkComplete();
        }
    }

    public void Init(string _text, float time)
    {
        label.text = _text;
        itemName = _text;
        strikeThrough.SetActive(false);
        patience = timeLeft = time;

    }

    public void MarkComplete()
    {
        strikeThrough.SetActive(true);
        label.color = completeCol;
        label.fontStyle = FontStyles.Italic;
    }

    public string GetItemName()
    {
        return itemName;
    }
}
