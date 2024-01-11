using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    ParticleSystem particleEffect;

    void Start()
    {
        particleEffect = gameObject.GetComponent<ParticleSystem>();
        particleEffect.Play(true);
    }

    public IEnumerator StartDashEffect()
    {
        particleEffect.Play(true);
         yield return new WaitForSeconds(0.4f);
        /*while (_vignetteIntensity > 0)
        {
            _vignetteIntensity -= 0.01f;
            if (_vignetteIntensity < 0) _vignetteIntensity = 0;
            _vignette.intensity.Override(_vignetteIntensity);
            yield return new WaitForSeconds(0.1f);

        }
        yield break;
        */
    }
}
