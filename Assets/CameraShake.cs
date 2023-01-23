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
        Vector2 offset = Vector2.zero;
        Vector2 original = transform.position;

        while (timeLeft >= 0) {
            float value = Mathf.Sin(Time.time * freq) * mag;
            offset = new Vector2(value, value);
            transform.position = original + offset;
            yield return new WaitForEndOfFrame();
            timeLeft -= Time.deltaTime;
            original = transform.position - (Vector3) offset;
        }
        transform.position = original;
    }
}
