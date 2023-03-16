using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CalendarEntryCoordinator : MonoBehaviour
{
    public GameObject x, present, cake;
    [SerializeField] bool showX, showPresent, showCake;

    private void Start()
    {
        x.SetActive(false);
    }

    private void Update()
    {
        if (Application.isPlaying) return;

        x.SetActive(showX);
        present.SetActive(showPresent);
        cake.SetActive(showCake);
    }
}
