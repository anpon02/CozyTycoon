using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    [SerializeField] GameObject choiceParent, optionPrefab;

    public void OpenChoiceUI(List<KitchenManager.ChoiceData.Option> options)
    {
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
    }
}
