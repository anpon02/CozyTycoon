using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DayNightController;

public class CalendarController : MonoBehaviour
{
    [SerializeField] List<CalendarEntryCoordinator> days = new List<CalendarEntryCoordinator>();
    [SerializeField] GameObject chef;
    [SerializeField] ChoiceController choice;

    private void OnEnable()
    {
        GameManager.instance.timeScript.PauseTime();

        int day = GameManager.instance.timeScript.day + 1;
        
        StartCoroutine(MoveChef(day));
    }

    IEnumerator MoveChef(int day)
    {
        var target = days[day].transform.position;
        yield return new WaitForSeconds(1f);

        while (Vector2.Distance(target, chef.transform.position) > 0.05f) {
            chef.transform.position = Vector3.Lerp(chef.transform.position, target, 0.05f);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < days.Count; i++) {
            if (i < day) days[i].x.SetActive(true);
            if (i < day) days[i].present.SetActive(false);
            if (i < day) days[i].cake.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        if (days[day].present.activeInHierarchy) DoPresent(day);
        else if (days[day].cake.activeInHierarchy) DoCake(day);
        else GameManager.instance.timeScript.UnpauseTime();

        yield return new WaitForSeconds(0.5f);

        transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void DoPresent(int day)
    {
        days[day].present.SetActive(false);
        choice.StartChoiceAnim();
    }

    void DoCake(int day)
    {
        days[day].cake.SetActive(false);
    }
}
