using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStatus : MonoBehaviour
{
    [Range(0, 1.0f)]
    [SerializeField] private float relationshipValue;

    [Tooltip("This is the value that will be added to the relationship value if the player provides a perfect dish." +
             "If the player gives the worst possible dish, this value will be subtracted from the relationship value." +
             "If a dish is good, but not perfect, a percentage of this value will be added based on the food quality value." +
             "If a dish is bad, but not the worst, a percentage of this value will be subtracted based on thefood quality value")]
    [Range(0, 1.0f)]
    [SerializeField] private float maxOrderValue;

    private CustomerParticle custParticle;

    // TODO: add maxInteractionValue that follows same principles as maxOrderValue

    private void Awake() {
        custParticle = transform.parent.GetComponentInChildren<CustomerParticle>();
    }

    public float GetRelationshipValue() {
        return relationshipValue;
    }

    public void SetRelationshipValue(float newValue) {
        if(newValue < 0 || newValue > 1.0f) {
            Debug.LogError("newValue must be a float between 0.0 and 1.0");
            return;
        }
        relationshipValue = Mathf.Round(newValue * 100f) / 100f;
    }

    public void GiveFood(float foodQualityValue) {
        /*
        // I'll get rid of this once we know it's really working
        print(foodQualityValue);
        if(foodQualityValue > 0.5f) {
            print("you gave the customer good food");
            float newValue = Mathf.Clamp(relationshipValue + (foodQualityValue * maxOrderValue), 0f, 1f);
            SetRelationshipValue(newValue);
        }
        else if(foodQualityValue < 0.5f) {
            print("you gave the customer bad food");
            float newFoodQuality = 1 - foodQualityValue;
            float newValue = Mathf.Clamp(relationshipValue - (newFoodQuality * maxOrderValue), 0f, 1f);
            SetRelationshipValue(newValue);
        }
        else
            print("you gave the customer mediocre food");
        */
        if(foodQualityValue >= KitchenManager.instance.midHighQualityCutoff.y) {
            print("you gave the customer good food");
            float newValue = Mathf.Clamp(relationshipValue + (foodQualityValue * maxOrderValue), 0f, 1f);
            SetRelationshipValue(newValue);
        }
        else if(foodQualityValue <= KitchenManager.instance.midHighQualityCutoff.x) {
            print("you gave the customer bad food");
            float newFoodQuality = 1 - foodQualityValue;
            float newValue = Mathf.Clamp(relationshipValue - (newFoodQuality * maxOrderValue), 0f, 1f);
            SetRelationshipValue(newValue);
        }
        else {
            print("you gave the customer mediocre food");
        }

        custParticle.EmitThumb(foodQualityValue);
    }
}
