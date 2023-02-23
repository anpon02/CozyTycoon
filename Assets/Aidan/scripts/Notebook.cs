using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notebook : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText, nameText;

    [SerializeField] List<string> notes = new List<string>();
    [SerializeField] List<CharacterName> names = new List<CharacterName>();
    [SerializeField] List<Image> buttons = new List<Image>();
    [SerializeField] Color inactiveColor;
    [SerializeField] GameObject parent, noteBookButton;
    bool firstNote;

    public void OpenNoteBook()
    {
        for (int i = 0; i < notes.Count; i++) {
            if (!string.IsNullOrEmpty(notes[i])) buttons[i].gameObject.SetActive(true);
            else buttons[i].gameObject.SetActive(false);
        }
        parent.SetActive(true);
        noteBookButton.SetActive(true);
    }

    private void Start()
    {
        GameManager.instance.notebook = this;
        mainText.text = nameText.text = "";
        parent.SetActive(false);
        noteBookButton.SetActive(false);
    }

    public void RecordInfo(string info, CharacterName character)
    {
        print("AHHHH: " + info);
        int index = 0;
        for (int i = 0; i < names.Count; i++) {
            if (names[i] == character) index = i;
        }
        notes[index] += info + "\n\n";
        if (!firstNote) FirstNote();
    }

    void FirstNote()
    {
        firstNote = true;
        GameManager.instance.Notify(callback: OpenNoteBook);
    }

    public void ShowText(int index)
    {
        SetName(index);
        SetButtonColor(index);
        mainText.text = notes[index];
    }

    void SetName(int index)
    {
        var character = names[index];
        string _name = character.ToString().ToUpper();
        nameText.text = _name;
    }

    void SetButtonColor(int index)
    {
        foreach (var b in buttons) b.color = inactiveColor;

        var col = buttons[index].color;
        col.a = 1;
        buttons[index].color = col; 
    }
}
