using System;
using UnityEngine;

public class CreditsMainMenuMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clipMain;
    public AudioClip clipCredits;

    private void OnEnable()
    {
        audioSource.clip = clipCredits; audioSource.Play();
    }
    private void OnDisable()
    {
        audioSource.clip = clipMain; audioSource.Play();
    }
   
}
