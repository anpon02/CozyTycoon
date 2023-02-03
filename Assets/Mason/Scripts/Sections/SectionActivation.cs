using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectionActivation : MonoBehaviour
{
    [SerializeField] private int sectionId;
    [SerializeField] private Color deactivatedColor;
    private SpriteRenderer sectionRenderer;
    private Tilemap sectionTilemap;         // any tilemap stuff may be deleted if we decide to not use tilemaps
    private Collider2D sectionTrigger;
    private Section thisSection;

    private void Awake() {
        sectionRenderer = GetComponent<SpriteRenderer>();
        sectionTilemap = GetComponent<Tilemap>();
        sectionTrigger = GetComponent<Collider2D>();
    }

    private void Start() {
        thisSection = SectionManager.instance.GetSectionById(sectionId);

        // deactivate this section if 
        if(thisSection.GetSectionId() != SectionManager.instance.GetCurrentSectionId())
            ChangeSectionColor(thisSection, deactivatedColor);
    }
/*
    private void OnTriggerEnter2D(Collider2D other) {
        print(gameObject + " " + other);
        if(other.tag == "Player") {
            int prevSectionId = SectionManager.instance.GetCurrentSectionId();
            Section prevSection = SectionManager.instance.GetSectionById(prevSectionId);

            // move camera
            SectionManager.instance.MoveSections(sectionId);

            // gray out previous section
            ChangeSectionColor(prevSection, deactivatedColor);

            // set new section's color to white
            ChangeSectionColor(thisSection, Color.white);
        }
    }
*/

    private void OnTriggerEnter2D(Collider2D other) {
        // return if retriggering thisSection's collider
        if(SectionManager.instance.GetCurrentSectionId() == thisSection.GetSectionId()) return;

        // switch camera to entered section and color it white
        if(other.tag == "Player") {
            SectionManager.instance.MoveSections(sectionId);
            ChangeSectionColor(thisSection, Color.white);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // return if staying in the same room
        if(SectionManager.instance.GetCurrentSectionId() == thisSection.GetSectionId()) return;

        // change section color to deactivated color
        if(other.tag == "Player") {
            ChangeSectionColor(thisSection, deactivatedColor);
        }
    }

    public SpriteRenderer GetSectionRenderer() {
        return sectionRenderer;
    }

    public Tilemap GetSectionTilemap() {
        return sectionTilemap;
    }

    private void ChangeSectionColor(Section section, Color newColor) {
        // change color of the section gameobject
        SpriteRenderer sprRenderer = section.GetSectionTransform().GetComponent<SectionActivation>().GetSectionRenderer();
        Tilemap tilemap = section.GetSectionTransform().GetComponent<SectionActivation>().GetSectionTilemap();

        if(sprRenderer)
            sprRenderer.color = newColor;
        if(tilemap)
            tilemap.color = newColor;

        // loop through and change color of child objects
        foreach(Transform child in section.GetSectionTransform()){
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            Tilemap childTilemap = child.GetComponent<Tilemap>();

            if(childRenderer)
                childRenderer.color = newColor;
            if(childTilemap)
                childTilemap.color = newColor;
        }
    }
}
