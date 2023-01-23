using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStory : MonoBehaviour
{
    [SerializeField] private TextAsset inkStory;
    private int storyPhaseNum;

    private void Awake() {
        storyPhaseNum = 0;
    }

    public int GetStoryPhaseNum() {
        return storyPhaseNum;
    }

    public void NextStoryPhase() {
        storyPhaseNum++;
    }

    public TextAsset GetInkStory() {
        return inkStory;
    }
}
