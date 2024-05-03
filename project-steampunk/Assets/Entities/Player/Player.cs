using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ITarget
{
    private Vector3 playerPosition;
    private bool isDead;
    [SerializeField] 
    private DamageTakenEffect damageEffect;
    [SerializeField]
    private DamageTakenEffect healEffect;
    public static Action dieMenuEvent;

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
        StartCoroutine(damageEffect.TakeDamageEffect());
    }
    public void HandlePlayerHeal(float damage)
    {
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
        isDead = true;
       dieMenuEvent?.Invoke();
        AudioManager.InstanceAudio.PlayMusic("Battle",false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
