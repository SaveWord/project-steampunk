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

    private LaserAttack _laser;
    [SerializeField]
    private float _laserAttackDuration;
    [SerializeField]
    private float _laserAttackChargeDuration;
    [SerializeField]
    private float _laserTimeout;

    [SerializeField]
    private GameObject _pointOfAttack;
    [SerializeField]
    private int _laserFollowDistance;
    [SerializeField]
    private float _laserDamage = 1;

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
    public bool bee = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _OnReload = true;
        if (_patternQueue.Count != 0)
        {
            foreach (GameObject child in _patternQueue)
            {
                _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._damage = _damagePattern;
                _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._damageTime = _damageTimePattern;
                _patternQueue[_attackQueue].GetComponent<GroundPatterns>()._chargeTime = _chargeTimePattern;
            }
        }
        _laser = _laserList.GetComponent<LaserAttack>();
        if (_laser)
        {
            
            _laser._followDistance = _laserFollowDistance;
            _laser._damage = _laserDamage;
            _laser._attackDuration = _laserAttackDuration;
            _laser._chargeDuration = _laserAttackChargeDuration;
            _laser._pointOfAttack = _pointOfAttack;
        }

    }


    public void Attack(ITarget target, List<AttackConstruct> attack)
    {
        if (bee)
        {
            if (_OnReload)
                StartCoroutine(Laser());
        }
        else
        {
            if (!instanciated)
            //   if (_laser)
            {
                // InvokeRepeating("Laser", 2, _laserAttackDuration + _laserTimeout);
                instanciated = true;
            }

            //transform.LookAt(target.GetPosition());
            if (_OnReload) //!_attack.Activated && 
            {

                _patternQueue[_attackQueue].GetComponent<GroundPatterns>().PulseCycle();
                //_attacksList.Add(_patternQueue[_attackQueue].GetComponent<GroundPatterns>());
                _animator.SetBool("isAttacking0", true);
                StartCoroutine(Reload(_patternQueue[_attackQueue]));
                _attackQueue++;
            }

            if (_attackQueue == _patternQueue.Count)
            {

                StartCoroutine(Laser());
                _attackQueue = 0;

            }
        }
    }


    private IEnumerator Reload(GameObject gameobject)
    {
        _OnReload = false;

        yield return new WaitForSeconds(_reloadTimePattern + _chargeTimePattern + _damageTimePattern);
        _OnReload = true;
         _animator.SetBool("isAttacking0", false);
    }

    private IEnumerator Laser()
    {

        _OnReload = false;
        yield return new WaitForSeconds(_laserTimeout);
        _laserList.SetActive(true);
        _animator.SetBool("isAttacking1", true);
        //  _laser.transform.eulerAngles = new Vector3(90, 0, 0);
        _laser.StartAttack();
        yield return new WaitForSeconds(_laserAttackDuration + _laserAttackChargeDuration);
        _animator.SetBool("isAttacking1", false);
        _OnReload = true;


    }
}