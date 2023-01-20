using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public static SectionManager instance;
    
    [SerializeField] private int currentSectionId;
    [SerializeField] private List<Section> sections;
    private MoveCamera camMovement;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);

        camMovement = Camera.main.GetComponent<MoveCamera>();
    }

    private void Start() {
        Camera.main.transform.position = sections[currentSectionId].GetCameraPosition();
    }

    // FOR TESTING PURPOSES
    private void Update() {
        if(Input.GetKeyDown(KeyCode.B)) {
            int newID = currentSectionId == 0 ? 1 : 0;
            MoveSections(newID);
        }
    }

    public List<Section> GetSections() {
        return sections;
    }

    public Section GetSectionById(int id) {
        foreach(Section section in sections)
            if(section.GetSectionId() == id)
                return section;
        return null;
    }

    public int GetCurrentSectionId() {
        return currentSectionId;
    }

    public void MoveSections(int newSectionId) {
        currentSectionId = newSectionId;
        camMovement.SetNewRoom(newSectionId);
    }
}
