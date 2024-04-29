using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageTakenEffect : MonoBehaviour
{
    [SerializeField] private float _vignetteIntensity = 0;
     Volume _volume;
    private Vignette _vignette;

    void Start()
    {
        //_volume = GetComponent<Volume>();
        _volume = gameObject.GetComponent<Volume>();
        if (_volume.profile.TryGet<Vignette>(out _vignette))

            if (!_vignette)
            Debug.Log("set up vignette");
        else _vignette.intensity.Override(0f);
    }

    public IEnumerator TakeDamageEffect()
    {

        _volume.priority = 111;
        _vignetteIntensity = 0.4f;
        ParticleSystem part = gameObject.GetComponent<ParticleSystem>();
        if(part)
            part.Play();
        _vignette.intensity.Override(0.4f);
        yield return new WaitForSeconds(0.4f);
        while (_vignetteIntensity > 0)
        {
            _vignetteIntensity -= 0.01f;
            if (_vignetteIntensity < 0) _vignetteIntensity = 0;
            _vignette.intensity.Override(_vignetteIntensity);
            yield return new WaitForSeconds(0.1f);
            _volume.priority = 98;
        }
        yield break;
    }
}
