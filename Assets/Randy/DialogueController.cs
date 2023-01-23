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
        // Contents in Update are for testing purposes while I only have access to the UI
        if (DialogueManager.instance.playerDistance < DialogueManager.instance.GetMinFadeoutThreshold() && !DialogueManager.instance.isFadingIn)
        {
            DialogueManager.instance.isFadingIn = true;
            DialogueManager.instance.isFadingOut = false;
            StartCoroutine(coordinator.PanelFadein());
        }
        else if (DialogueManager.instance.playerDistance > DialogueManager.instance.GetMinFadeoutThreshold() && !DialogueManager.instance.isFadingOut)
        {
            DialogueManager.instance.isFadingIn = false;
            DialogueManager.instance.isFadingOut = true;
            StartCoroutine(coordinator.PanelFadeout());
        }
    }

    public void StartDialogue(TextAsset inkStory)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue();
    }

    public void StartDialogue(TextAsset inkStory, string knotName)
    {
        coordinator.LoadCharacterStory(inkStory);
        coordinator.StartDialogue(knotName);
    }
}
