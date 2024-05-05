using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager InstanceAudio;

    public Sound[] music, sfxSound, sfxSoundWeapon;

    public AudioSource musicSource, sfxSource, sfxWeaponSource, sfxEnemySource;
    private void Awake()
    {
        if (InstanceAudio == null)
        {
            InstanceAudio = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void PlaySfxWeapon(string name)
    {
        Sound sfx = Array.Find(sfxSoundWeapon, x => x.name == name);
        if (!sfxWeaponSource.isPlaying && sfx.name == "ChangeWeapon")
            sfxWeaponSource.PlayOneShot(sfx.clip);
        else if (sfx.name != "ChangeWeapon")
            sfxWeaponSource.PlayOneShot(sfx.clip);
    }

    public void PlaySfxEnemy(string name)
    {
        Sound sfx = Array.Find(sfxSound, x => x.name == name);
        sfxEnemySource.PlayOneShot(sfx.clip);
    }

    public void PlaySfxSound(string name)
    {
        Sound sfx = Array.Find(sfxSound, x => x.name == name);
        if (!sfxSource.isPlaying && sfx.name == "Move")
            sfxSource.PlayOneShot(sfx.clip);
        else if (sfx.name != "Move")
            sfxSource.PlayOneShot(sfx.clip);
    }
    public void PlayMusic(string name, bool musicState)
    {
        Sound ms = Array.Find(music, x => x.name == name);
        switch (musicState)
        {
            case true:
                musicSource.clip = ms.clip;
                musicSource.Play();
                break;
            case false:
                StartCoroutine(FadeOut(musicSource, 0.5f));
                break;
        }
    }
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}