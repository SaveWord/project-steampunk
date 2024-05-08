using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSoundLocation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _system;
    [SerializeField] private float timeParticlePlay;
    [SerializeField] private AudioSource _source;
    private bool _isPlaying = true;
    private void Awake()
    {
        //почему то не хочет подхватывать компонент, пока прокинул в инспекторе.
        //_system.GetComponent<ParticleSystem>();
        //_source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        InvokeRepeating("ParticlePlay", 0, timeParticlePlay);
    }
    private void ParticlePlay()
    {
        if (_isPlaying)
        {
            _system.Play();
            _source.Play();
            _isPlaying = false;
        }
        else
        {
            _system.Stop();
            _source.Stop();
            _isPlaying = true;
        }

    }
}
