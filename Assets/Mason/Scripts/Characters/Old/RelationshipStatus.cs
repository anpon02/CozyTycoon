using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStatus : MonoBehaviour
{
    [SerializeField] private CharacterName customerName;
    public int relationshipValue;
    CustomerParticle custParticle;
    
    private void Awake() {
        custParticle = transform.parent.GetComponentInChildren<CustomerParticle>();
    }

    public float GetRelationshipValue() {
        return relationshipValue;
    }

    public void updateRelationshipValue(int newValue) {
        relationshipValue += newValue;
        relationshipValue = Mathf.Max(0, relationshipValue);

        custParticle.Emit(newValue);

        PlayerPrefs.SetInt(customerName.ToString().ToLower() + "Status", relationshipValue);
        PlayerPrefs.Save();
    }
}
