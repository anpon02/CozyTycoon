using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerParticle : MonoBehaviour
{
    ParticleSystem pSystem;

    private void Awake() {
        pSystem = GetComponent<ParticleSystem>();
    }

    private void Start() {
        pSystem.Stop();
    }

    public void Emit(float value) 
    {
        if (value <= 0) return;

        pSystem.Play();
        StartCoroutine(StopParticles());
    }

    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(0.5f);
        pSystem.Stop();
    }
}
