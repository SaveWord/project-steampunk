/*using Enemies;
using Enemies.Attacks.Attacks;
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class RingTargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    [SerializeField] RangeAttack _attackRegular;
    [SerializeField] RingAttack _attackRing;
    [SerializeField] RangeAttack _attackPattern;
    [SerializeField] RangeAttack _attackPattern1;
    //[SerializeField] IAttack _attackLaser;
    //[SerializeField] private List<Pair<IAttack, Transform>> _attackQueue;
    RangeAttack _attack;
    //  IAttack _attack = new();
    int _attackOrder = 0;
    int _attackCount;
    [SerializeField] private Transform _attackGroundSpot;
    [SerializeField] private Transform _attackGunSpot;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private float _reloadTime1 = 10f;
    private Animator _animator;

    private bool _OnReload;

    private void Awake()
    {
        //_attackCount = _attackQueue.Count;
        _animator = GetComponentInChildren<Animator>();
        _OnReload = true;
        _attackRing = Instantiate(_attackRing, transform);
        _attack = Instantiate(_attackRegular, transform);
        InvokeRepeating("AttackRing", 2, _reloadTime1);
        //InvokeRepeating("Attack", 1, _reloadTime);
        _attackPattern = Instantiate(_attackPattern, transform);
        _attackPattern1 = Instantiate(_attackPattern1, transform);
        _attackRegular = Instantiate(_attackRegular, transform);
    }

    public void SetAttack()
    {
        _attackOrder += 1;
        // var _attacko = _attackQueue[_attackOrder].Value;

        

        if (_attackOrder == 3) _attackOrder = 1;


    }

    public void Attack(ITarget target, RangeAttack attack)
    {
        transform.LookAt(target.GetPosition());
        switch (_attackOrder)
        {
            case 1:
                if (!_attackPattern.Activated)
                {
                    _attackPattern.Activate(target, _attackGunSpot);
                    //_animator.SetBool("isAttacking", true);
                    StartCoroutine(Reload(_reloadTime));
                }
                break;
            case 2:
                if (!_attackPattern1.Activated)
                {
                    _attackPattern1.Activate(target, _attackGunSpot);
                    //_animator.SetBool("isAttacking", true);
                    StartCoroutine(Reload(_reloadTime));
                }
                break;
            case 3:
                if (!_attackPattern.Activated)
                {
                    _attackPattern.Activate(target, _attackGunSpot);
                    //_animator.SetBool("isAttacking", true);
                    _attackOrder = 1;
                }
                break;
            // case 4:
            //     _attack = Instantiate(_attack, transform);
            //     break;
            default:
                break;
        }
    }
    public void AttackRing()
    {
        _attackRing.Activate(null, _attackGroundSpot);

    }
    private IEnumerator Reload(float reloadTime)
    {
        _OnReload = false;
        yield return new WaitForSeconds(reloadTime);
        _OnReload = true;
        //_animator.SetBool("isAttacking", false);
    }


}*/
