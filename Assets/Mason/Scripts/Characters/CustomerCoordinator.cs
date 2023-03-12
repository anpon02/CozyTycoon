using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerCoordinator : MonoBehaviour
{
    [Header("Relationship Status")]
    public int relationshipValue;

    [Header("Customer Story")]
    public CharacterName characterName;
    [SerializeField] private TextAsset inkStory;
    private int storyPhaseNum;
    private bool storyStarted;
    [HideInInspector] public bool storyFinished;

    [Header("Particle System")]
    [SerializeField] private float emitTime;
    private ParticleSystem pSystem;

    [Header("Customer Interactable")]
    [SerializeField] private CustomerInteractable forkKnife;
    private CustomerMovement movement;
    private CustomerOrderController orderController;

    private void Awake() {
        // Customer Story
        storyPhaseNum = 0;
        storyStarted = false;

        // Customer Particle
        pSystem = GetComponentInChildren<ParticleSystem>();

        // customer interactable
        movement = GetComponent<CustomerMovement>();
        orderController = GetComponentInChildren<CustomerOrderController>();
    }

    private void Start() {
        // Customer Particle
        pSystem.Stop();
    }

    private void Update() {
        if(PauseManager.instance && PauseManager.instance.paused) return;

        // activate/deactivate forkKnife based on movement and if food is delivered
        if(!movement.IsMoving() && !orderController.GetHasReceivedFood())
            forkKnife.gameObject.SetActive(true);
        else
            forkKnife.gameObject.SetActive(false);

        if (storyStarted && !storyFinished) CheckForStoryEnd();
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
        if (!DialogueManager.instance || DialogueManager.instance.StoryDisabled(characterName)) return;

        DialogueManager.instance.speakingCharacter = gameObject;
        DialogueManager.instance.StartDialogueMainStory(inkStory, characterName, storyPhaseNum);
        storyStarted = true;
        NextStoryPhase();
    }

    public void StartEnding() {
        DialogueManager.instance.StartDialogueEnding(inkStory, characterName, !DialogueManager.instance.StoryDisabled(characterName));
    }

    public bool GetStorySaid() {
        return storyStarted;
    }

    public void SetStorySaid(bool said) {
        storyStarted = said;
    }

    void CheckForStoryEnd()
    {
        if (DialogueManager.instance.StoryEnded()) storyFinished = true;
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
