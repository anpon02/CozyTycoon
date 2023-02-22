using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerCoordinator : MonoBehaviour
{
    [HideInInspector] public bool inRestaurant;

    [Header("Relationship Status")]
    public int relationshipValue;

    [Header("Customer Story")]
    public CharacterName characterName;
    [SerializeField] private TextAsset inkStory;
    private int storyPhaseNum;
    private bool storySaid;

    [Header("Particle System")]
    [SerializeField] private float emitTime;
    private ParticleSystem pSystem;

    private void Awake() {
        // Customer Story
        storyPhaseNum = 0;
        storySaid = false;

        // Customer Particle
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        // Customer Particle
        pSystem.Stop();
    }

    /*
    * RELATIONSHIP STATUS FUNCTIONS
    */
    public float GetRelationshipValue() {
        return relationshipValue;
    }

    public void updateRelationshipValue(int newValue) {
        relationshipValue += newValue;
        relationshipValue = Mathf.Max(0, relationshipValue);

        Emit(newValue);

        PlayerPrefs.SetInt(characterName.ToString().ToLower() + "Status", relationshipValue);
        PlayerPrefs.Save();
    }

    /*
    * CUSTOMER STORY FUNCTIONS
    */
    public int GetStoryPhaseNum() {
        return storyPhaseNum;
    }

    public void NextStoryPhase() {
        storyPhaseNum++;
    }

    public TextAsset GetInkStory() {
        return inkStory;
    }

    public void StartStory() {
        if (!DialogueManager.instance) return;

        DialogueManager.instance.speakingCharacter = gameObject;
        DialogueManager.instance.StartDialogueMainStory(inkStory, characterName, storyPhaseNum);
        storySaid = true;
        NextStoryPhase();
    }

    public bool GetStorySaid() {
        return storySaid;
    }

    public void SetStorySaid(bool said) {
        storySaid = said;
    }

    /*
    * CUSTOMER PARTICLE FUNCTIONS
    */
    public void Emit(float value) 
    {
        if (value <= 0) return;

        pSystem.Play();
        StartCoroutine(StopParticles());
    }

    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(emitTime);
        pSystem.Stop();
    }
}
