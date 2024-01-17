using Enemies;
using Enemies.Attacks.AttackUnits;
using System.Collections;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [SerializeField] private AttackUnit _attackUnit;
    [SerializeField] private Transform _attackSpawnPoint;
    [SerializeField] private int _maxBlockedAttaksCount = 3;

    private bool _readyToAttack;

    private void Awake()
    {
        _readyToAttack = true;
        _attackUnit.AttackSpawnPoint = _attackSpawnPoint;
    }

    public void SetAttackUnit(AttackUnit attackUnit)
    {
        _attackUnit = attackUnit;
    }

    public void Attack(ITarget target)
    {
        transform.LookAt(target.GetPosition());

        if (_readyToAttack)
        {
            _attackUnit.Attack(target);
            StartCoroutine(Reload());
        }
    }

    public bool ShouldChangePosition()
    {
        return _attackUnit.BlockedAttacksCount >= _maxBlockedAttaksCount;
    }

    private IEnumerator Reload()
    {
        _readyToAttack = false;
        yield return new WaitForSeconds(2f);
        _readyToAttack = true;
    }
   

}
