using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Enemies;

public class HpBoss : MonoBehaviour
{
    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private GameObject healDropPrefab;

    //vfxGraphTakeDamage
    private VisualEffect enemyDamageImpact;

    //delete ListSpawner and check door
    public int _idEnemy;
    public event Action<int> DeleteList;

    [SerializeField] private EnemyAudioCollection _audioSource;
    [SerializeField] private Animator _animator;

    public static event Action OnBossDefeated = delegate { };

    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleEnemyDied;
        GetComponent<IHealth>().ChangeVfxImpact += HandleVfxPos;

        enemyDamageImpact = GetComponentInChildren<VisualEffect>();
        healDropPrefab = Resources.Load<GameObject>("HealDrop");

        _audioSource = gameObject.GetComponentInChildren<EnemyAudioCollection>();
    }

    private void HandleEnemyTakenDamage()
    {
        if (_audioSource != null)
            _audioSource.PlaySfxEnemy("EnemyDamaged");
        if (gameObject.GetComponent<TargetDetector>() != null)
            gameObject.GetComponent<TargetDetector>().GetShot();
    }

    private void HandleVfxPos(Vector3 pos)
    {
        Vector3 direction = pos.normalized;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        enemyDamageImpact.transform.position = pos;
        enemyDamageImpact.transform.rotation = rotation;
        enemyDamageImpact.Play();
    }

    private void HandleEnemyDied()
    {
        //var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        //animation of death
        _animator.enabled = true;
        _animator.SetBool("isDead", true);

        // GameObject.Destroy(this.gameObject, 0.5f);
        // DeleteList(_idEnemy);

        //Destroy(deathparticle, 2.5f); 
    }

    public void BossDied()
    {
        Debug.Log("boss died ^(");
        if (_audioSource != null)
            _audioSource.PlaySfxEnemy("EnemyDeath");
        OnBossDefeated();
        GameObject.Destroy(this.gameObject);
    }
}
