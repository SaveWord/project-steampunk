using Enemies;
using Enemies.Attacks.Attacks;
using Enemies.Bullets;
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Enemies.Attacks.Attacks;
using Enemies.BossStates;

public class ArenaAttacker : MonoBehaviour, IBossTargetAttacker
{
    [Header("Basics")]
    //private RangeAttack _attack;
    private int _attackQueue = 0;
    bool instanciated = false;
    private Animator _animator;
    private List<GroundPatterns> _attacksList;
    private bool _OnReload;
    
    private StateMachine _stateMachine;

    [Header("Laser settings")]
    [SerializeField] 
    private GameObject _laserList;

    private LaserFollow _laser;
    [SerializeField]
    private float _laserAttackDuration;
    [SerializeField]
    private float _laserTimeout;


    [SerializeField]
    private GameObject _pointOfAttack;
    [SerializeField]
    private float _laserFollowDistance;

    [SerializeField]
    private float _laserDamage = 1;
    [SerializeField]
    private float _laserSpeed;

    [Header("Patterns settings")]
    
    [SerializeField]
    private List<GameObject> _patternQueue;
    [SerializeField] 
    private float _damagePattern;
    [SerializeField] 
    public float _damageTimePattern;
    [SerializeField] 
    private float _chargeTimePattern;
    [SerializeField]
    public float _reloadTimePattern = 4f;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _OnReload = true;
        foreach (GameObject child in _patternQueue)
        {
            _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._damage = _damagePattern;
            _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._damageTime = _damageTimePattern;
            _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._chargeTime = _chargeTimePattern;
        }

        _laser = _laserList.GetComponent<LaserFollow>();
        _laser.LaserFollowInstanciate( _pointOfAttack,_laserFollowDistance, _laserDamage,  _laserSpeed,  _laserAttackDuration);
        
    }

    public void SetAttack(List<AttackConstruct> attack)
    {
        _laserList.SetActive(true);
        _laser = _laserList.GetComponent<LaserFollow>();
        //DestroyLaser(_laserFireTime);

        instanciated = true;
    }

    public void Attack(ITarget target, List<AttackConstruct> attack)
    {
        //if (!instanciated)
        //     SetAttack(attack);

        //transform.LookAt(target.GetPosition());
        if (_OnReload) //!_attack.Activated && 
        {
            
            _patternQueue[_attackQueue].GetComponent<GroundPatterns>().PulseCycle();
            //_attacksList.Add(_patternQueue[_attackQueue].GetComponent<GroundPatterns>());
            _animator.SetBool("isAttacking", true);
            StartCoroutine(Reload(_patternQueue[_attackQueue]));
            _attackQueue++;
        }
        
        if (_attackQueue == _patternQueue.Count)
        { 
            _attackQueue = 0;
            if (_laser)
                StartCoroutine(Laser(target)); 
            InvokeRepeating("Laser", 2, _laserAttackDuration + _laserTimeout);
        }
    }


    private IEnumerator Reload(GameObject gameobject)
    {
        _OnReload = false;

        yield return new WaitForSeconds(_reloadTimePattern+ _chargeTimePattern+ _damageTimePattern);
        _OnReload = true;
        // _animator.SetBool("isAttacking", false);
    }

    private IEnumerator Laser(ITarget target)
    {
        yield return new WaitForSeconds(_laserTimeout);
        _laserList.SetActive(true);
        _laser.AttackCycle(target);
    }
}