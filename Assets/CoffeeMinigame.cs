using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMinigame : MonoBehaviour
{
    [SerializeField] WorkstationUICoordinator uiCoord;
    [SerializeField] int buttonSound, failSound, winTimeRequirement = 6, patternStartLength = 3;
    [SerializeField] float patternDisplayDelay = 0.5f;
    [SerializeField] Color failColor;
    [SerializeField] List<Image> buttons = new List<Image>();
    [SerializeField] List<Color> highlightColors = new List<Color>();

    List<int> pattern = new List<int>(), inputPattern = new List<int>();
    Color originalButtonColor;

    public void PressButton(int index)
    {
        AudioManager.instance.PlaySound(buttonSound, buttons[index].gameObject);
        inputPattern.Add(index);
        if (!PatternsMatch()) { StartCoroutine(FailPattern()); return;}
        else uiCoord.AddProgress(1 / (float) winTimeRequirement);

        if (inputPattern.Count == pattern.Count) StartCoroutine(AddToPattern());
    }

    bool PatternsMatch()
    {
        for (int i = 0; i < inputPattern.Count; i++) {
            if (inputPattern[i] != pattern[i]) return false;
        }
        return true;
    }

    IEnumerator FailPattern()   
    {
        DisablePlayerInput();
        inputPattern.Clear();
        pattern.Clear();
        AudioManager.instance.PlaySound(failSound, gameObject);
        foreach (var b in buttons) b.color = failColor;
        yield return new WaitForSeconds(1.5f);
        foreach (var b in buttons) b.color = originalButtonColor;

        StartNewPattern();
    }

    private void Start()
    {
        originalButtonColor = buttons[0].color;
    }

    private void OnEnable()
    {
        uiCoord.ongoingMinigames += 1;
        StartNewPattern();
    }

    void StartNewPattern()
    {
        pattern.Clear();
        for (int i = 0; i < patternStartLength - 1; i++) {
            pattern.Add(Random.Range(0, buttons.Count));
        }
        StartCoroutine(AddToPattern());
    }

    IEnumerator AddToPattern()
    {
        if (uiCoord.progressSlider.value >= 1) {
            Complete();
            yield break;
        }

        yield return new WaitForSeconds(0.1f);
        pattern.Add(Random.Range(0, buttons.Count));
        DisplayPattern();
    }

    void DisplayPattern()
    {
        DisablePlayerInput();
        StartCoroutine(LightUpButtons());
    }

    void DisablePlayerInput()
    {
        foreach (var b in buttons) b.GetComponent<Button>().enabled = false;
    }

    IEnumerator LightUpButtons()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (var index in pattern) {
            LightUpButton(index);
            yield return new WaitForSeconds(patternDisplayDelay);
            buttons[index].color = originalButtonColor;
            yield return new WaitForSeconds(0.1f);
        }
        EnablePlayerInput();
    }

    void LightUpButton(int index)
    {
        print("INDEX: " + index);
        buttons[index].color = highlightColors[index];
        AudioManager.instance.PlaySound(buttonSound, buttons[index].gameObject);
    }

    void EnablePlayerInput()
    {
        foreach (var b in buttons) b.GetComponent<Button>().enabled = true;
        inputPattern.Clear();
    }

    void Complete()
    {
        uiCoord.ongoingMinigames -= 1;
        gameObject.SetActive(false);
        uiCoord.CompleteRecipe();
    }
}
