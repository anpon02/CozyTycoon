using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerParticle : MonoBehaviour
{
    [Range(0, 1f)]
    [SerializeField] private float positiveThreshold;
    [Range(0, 1f)]
    [SerializeField] private float negativeThreshold;
    [SerializeField] private Material positive;
    [SerializeField] private Material negative;
    private ParticleSystem pSystem;
    ParticleSystemRenderer pSystemRenderer;

    private void Awake() {
        pSystem = GetComponent<ParticleSystem>();
        pSystemRenderer = GetComponent<ParticleSystemRenderer>();
    }

    private void Start() {
        pSystem.Stop();
    }

    public void EmitThumb(float foodVal) {
        if(foodVal >= KitchenManager.instance.midHighQualityCutoff.y) {
            pSystemRenderer.material = positive;
            pSystem.Emit(1);
        }
        else if(foodVal <= KitchenManager.instance.midHighQualityCutoff.x) {
            pSystemRenderer.material = negative;
            pSystem.Emit(1);
        }
    }
}
