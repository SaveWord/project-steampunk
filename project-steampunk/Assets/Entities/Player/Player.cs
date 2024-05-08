using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour, ITarget
{
    private Vector3 playerPosition;
    private bool isDead;
    [SerializeField] 
    private DamageTakenEffect damageEffect;
    [SerializeField]
    private DamageTakenEffect healEffect;
    public  UnityEvent OnPlayerDeath;

    [SerializeField]
    private GameObject positionObject;

    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }

    public Vector3 GetPosition()
    {
        return positionObject.transform.position;
    }
    
    public void HandlePlayerDamage(float damage)
    {
        AudioManager.InstanceAudio.PlaySfxSound("PlayerDamaged");
        StartCoroutine(damageEffect.TakeDamageEffect());
    }
    public void HandlePlayerHeal(float damage)
    {
        AudioManager.InstanceAudio.PlaySfxSound("PlayerHealed");
        StartCoroutine(healEffect.TakeDamageEffect());
    }
    private void Start()
    {
        isDead = false;
        GetComponent<IHealth>().OnDied += HandlePlayerDied;
        GetComponent<IHealth>().OnTakenDamage += HandlePlayerDamage;
        GetComponent<IHealth>().OnHealedDamage += HandlePlayerHeal;
    }

    private void HandlePlayerDied()
    {
        if(!isDead)
            AudioManager.InstanceAudio.PlaySfxSound("PlayerDeath");
        isDead = true;
        OnPlayerDeath?.Invoke();
        AudioManager.InstanceAudio.musicSource.Stop();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
