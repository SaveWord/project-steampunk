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

    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public void HandlePlayerDamage(float damage)
    {
        StartCoroutine(damageEffect.TakeDamageEffect());
    }
    private void Start()
    {
        isDead = false;
        GetComponent<IHealth>().OnDied += HandlePlayerDied;
        GetComponent<IHealth>().OnTakenDamage += HandlePlayerDamage;
    }

    private void HandlePlayerDied()
    {
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
