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
    [SerializeField] private EnemyAudioCollection _audioSource ;

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

        _audioSource = gameObject.GetComponentInChildren<EnemyAudioCollection>();
    }
    private void HandleEnemyTakenDamage()
    {
        _audioSource.PlaySfxEnemy("EnemyDamaged");
        if (gameObject.GetComponent<TargetDetector>() != null)
            gameObject.GetComponent<TargetDetector>().GetShot();
        enemyDamageImpact.Play();
    }

    private void HandleEnemyDied()
    {
        //var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
        _animator.SetBool("isDead", true);
        _audioSource.PlaySfxEnemy("EnemyDeath");
        DeleteList(_idEnemy);
        //Destroy(deathparticle, 2.5f); 
        GameObject.Destroy(this.gameObject, 0.5f);

        // drop the heals
        var healCount = UnityEngine.Random.Range(1, 2);
        Debug.Log("Healed num " +healCount);
        for (int i=0; i <= healCount; i++)
        {

            var position = new Vector3(transform.position.x+ UnityEngine.Random.Range(-10, 10), transform.position.y+2, transform.position.z+ UnityEngine.Random.Range(-10, 10));

            Instantiate(healDropPrefab, position, Quaternion.identity);
        }
        
       
    }
}
