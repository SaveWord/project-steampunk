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

    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }

    public Vector3 GetPosition()
    {
        return Camera.main.transform.position;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
