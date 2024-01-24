using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HpHandler: MonoBehaviour, IHealth
{
    [SerializeField]
    private int _maxHp = 100;
    [SerializeField]
    private int _currentHp;
    [SerializeField]
    private bool _invulnerable = false;

    public float CurrentHp { get { return (float)_currentHp; } }

    public event Action<float> OnHPChanged = delegate { };

    private void Start()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(int amount)
    {
        if (!_invulnerable)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

            _currentHp -= amount;

            OnHPChanged(CurrentHp);

            if (_currentHp <= 0)
                Die();
        }
    }

    public void Heal(int amount)
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
        //OnDied();
        GameObject.Destroy(this.gameObject);
    }

    //if needed blood particle

    /*[SerializeField] private ParticleSystem deathParticlePrefab;
    public event Action OnDied = delegate { };
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