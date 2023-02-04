using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float defaultDur;
    [SerializeField] float defaultFreq;
    [SerializeField] float defaultMag;

    private void Start()
    {
        GameManager.instance.SetShakeScript(this);
    }

    public void Shake()
    {
        Shake(defaultDur, defaultFreq, defaultMag);
    }

    public void Shake(float duration, float freq, float mag)
    {
        StartCoroutine(_Shake(duration, freq, mag));
    }

    IEnumerator _Shake(float duration, float freq, float mag)
    {
        float timeLeft = duration;
        Vector3 offset = Vector3.zero;
        Vector3 original = transform.position;

        while (timeLeft >= 0) {
            float value = Mathf.Sin(Time.time * freq) * mag;
            offset = new Vector3(value, value, 0);
            transform.position = original + offset;
            yield return new WaitForEndOfFrame();
            timeLeft -= Time.deltaTime;
            original = transform.position - offset;
        }
        transform.position = original;
    }
}
