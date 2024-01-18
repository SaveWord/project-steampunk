using Enemies;
using Enemies.Attacks.AttackUnits;
using System.Collections;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [SerializeField] private AttackUnit _attackUnit;
    [SerializeField] private Transform _attackSpawnPoint;
    [SerializeField] private float _reloadTime = 2f;

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
        return true;
    }

    private IEnumerator Reload()
    {
        _readyToAttack = false;
        yield return new WaitForSeconds(_reloadTime);
        _readyToAttack = true;
    }
   

}
