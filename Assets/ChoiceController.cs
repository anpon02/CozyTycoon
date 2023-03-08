using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    [SerializeField] GameObject choiceParent, optionPrefab;
    List<KitchenManager.ChoiceData.Option> options = new List<KitchenManager.ChoiceData.Option>();
    bool ready;
    public void OpenChoiceUI(List<KitchenManager.ChoiceData.Option> _options)
    {
        ready = true;
        
        options = _options;
    }

    public void StartChoiceAnim()
    {
        if (!ready) return;
        ready = false;

        gameObject.SetActive(true);
        foreach (var o in options) {
            var newOpt = Instantiate(optionPrefab, choiceParent.transform);
            newOpt.GetComponentInChildren<ChoiceOptionCoordinator>().Init(o);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < choiceParent.transform.childCount; i++) {
            Destroy(choiceParent.transform.GetChild(i).gameObject);
        }
        GameManager.instance.timeScript.UnpauseTime();
    }
}
