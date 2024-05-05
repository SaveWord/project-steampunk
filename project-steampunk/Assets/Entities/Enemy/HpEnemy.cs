using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Enemies;

public class HpEnemy : MonoBehaviour
{
    private string state;
    private bool immovable;
    private Vector3 playerPosition;

    private float jumpForce;
    private Animator _animator;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private GameObject healDropPrefab;

    //vfxGraphTakeDamage
    private VisualEffect enemyDamageImpact;

    //delete ListSpawner and check door
    public int _idEnemy;
    public event Action<int> DeleteList;

     private void OnTriggerStay(Collider other)
     {
          if (immovable)
          {
              transform.position = playerPosition;
          }
     } 

    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleEnemyDied;
        _animator = GetComponentInChildren<Animator>();
        enemyDamageImpact = GetComponentInChildren<VisualEffect>();
        healDropPrefab = Resources.Load<GameObject>("HealDrop");
    }
    private void HandleEnemyTakenDamage()
    {
        AudioManager.InstanceAudio.PlaySfxSound("EnemyDamaged");
        if (gameObject.GetComponent<TargetDetector>() != null)
            gameObject.GetComponent<TargetDetector>().GetShot();
        enemyDamageImpact.Play();
    }

    private void HandleEnemyDied()
    {
        //var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
        _animator.SetBool("isDead", true);
        AudioManager.InstanceAudio.PlaySfxSound("EnemyDeath");
        DeleteList(_idEnemy);
        //Destroy(deathparticle, 2.5f); 
        GameObject.Destroy(this.gameObject, 1.5f);

        // drop the heals
        var healCount = UnityEngine.Random.Range(1, 2);
        Debug.Log("Healed num " +healCount);
        for (int i=0; i <= healCount; i++)
        {

            var position = new Vector3(transform.position.x+ UnityEngine.Random.Range(-10, 10), transform.position.y, transform.position.z+ UnityEngine.Random.Range(-10, 10));

            Instantiate(healDropPrefab, position, Quaternion.identity);
        }
        
       
    }
}
