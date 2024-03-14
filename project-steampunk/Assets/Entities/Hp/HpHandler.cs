using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enemies;

public class HpHandler: MonoBehaviour, IHealth
{
    [SerializeField]
    private float _maxHp = 100f;
    [SerializeField]
    private float _currentHp;
    [SerializeField]
    private bool _invulnerable = false;

    public float CurrentHp { get { return (float)_currentHp; } }

    public event Action<float> OnHPChanged = delegate { };
    public event Action<float> OnTakenDamage = delegate { };
    public event Action OnDied = delegate { };

    private void Start()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(float amount)//TODO: specify damage maker
    {
        //gameObject.GetComponent<TargetDetector>().GetShot();
        if (!_invulnerable)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

            _currentHp -= amount;

            OnHPChanged(CurrentHp);
            OnTakenDamage(amount);

            if (_currentHp <= 0)
                Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Invalid Heal amount specified: " + amount);

        else if (amount >= (_maxHp - _currentHp))
            _currentHp = _maxHp;
        else
            _currentHp += amount;

        OnHPChanged(CurrentHp);
    }

    private void Die()
    {
        OnDied();
        Debug.Log("died");
    }

    //if needed blood particle

    /*[SerializeField] private ParticleSystem deathParticlePrefab;
    private void Start()
    {
        GetComponent<IHealth>().OnDied += PlayDeathParticle;
    }
    private void PlayDeathParticle()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation);
        Destroy(deathparticle, 4f);
    }*/
}