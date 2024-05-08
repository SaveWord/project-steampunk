using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioCollection : MonoBehaviour
{
    public Sound[] sfxSound;
    public AudioSource sfxSource;
    public AudioSource sfxLaserSource;

    public void PlaySfxEnemy(string name)
    {
        Sound sfx = Array.Find(sfxSound, x => x.name == name);
        sfxSource.PlayOneShot(sfx.clip);
    }
    public void PlaySfxEnemyLaser()
    {
        sfxLaserSource.Play();
    }
    public void StopSfxEnemyLaser()
    {
        sfxLaserSource.Stop();
    }
}
