using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ITarget
{
    //[SerializeField] private Transform playerTransform;
    private Vector3 playerPosition;
    private bool isDead;
 [SerializeField] private DamageTakenEffect damageEffect;

    public int GetTargetID()
    {
        return gameObject.GetInstanceID();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    /*
    public void GetDamage(float damage)
    {
        if (timeNextAttack < Time.time)
        {
            timeNextAttack = Time.time + timeIntervalAttack;
            hp -= damage;
            StartCoroutine(damageEffect.TakeDamageEffect());
            hpUI.fillAmount = hp;
            if (hp <= 0.01)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
    */
    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleEnemyDied;
    }

    private void HandleEnemyDied()
    {
       // var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
        //Destroy(deathparticle, 4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
