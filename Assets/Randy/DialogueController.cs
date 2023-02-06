using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueCoordinator coordinator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogueManager.instance && DialogueManager.instance.GetSpeakingCharacter()) {
            float distance = Vector3.Distance(DialogueManager.instance.GetPlayer().transform.position, DialogueManager.instance.GetSpeakingCharacter().transform.position);
            DialogueManager.instance.SetPlayerDistance(distance);
            
            if(DialogueManager.instance.GetForcedVisibility())
            {
                DialogueManager.instance.SetFadeRate(DialogueManager.instance.GetFadeinRate() * 0.25f); 
            }
            else if (distance < DialogueManager.instance.GetMinFadeoutThreshold())
            {
                DialogueManager.instance.isFadingIn = true;
                DialogueManager.instance.isFadingOut = false;
                //StartCoroutine(coordinator.PanelFadein());
                DialogueManager.instance.SetFadeRate(DialogueManager.instance.GetFadeinRate());
            }
            else if (distance > DialogueManager.instance.GetMinFadeoutThreshold())
            {
                DialogueManager.instance.isFadingIn = false;
                DialogueManager.instance.isFadingOut = true;
                //StartCoroutine(coordinator.PanelFadeout());
                float fadeoutValue = DialogueManager.instance.GetFadeoutRate() * DialogueManager.instance.GetFadeoutRateMultiplier(distance);
                DialogueManager.instance.SetFadeRate(-fadeoutValue);
            }
        }
    }

    public void StartDialogue(TextAsset inkStory)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue();
    }

    public void StartDialogue(TextAsset inkStory, int progress)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(progress);
    }

    public void StartDialogue(TextAsset inkStory, string knotName)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(knotName);
    }

    public void StopDialogue()
    {
        coordinator.StopDialogue();
    }
}
