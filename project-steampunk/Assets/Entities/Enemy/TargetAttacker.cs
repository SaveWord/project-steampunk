using Enemies;
using Enemies.Attacks.AttackUnits;
using System.Collections;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    [SerializeField] private AttackUnit _attackUnit;
    [SerializeField] private Transform _attackSpawnPoint;
    [SerializeField] private float _reloadTime = 2f;

    private bool _isAttackCharged;


    private void Awake()
    {
        _isAttackCharged = true;
        _attackUnit = Instantiate(_attackUnit, transform);
        _attackUnit.AttackSpawnPoint = _attackSpawnPoint;
    }

    public void SetAttackUnit(AttackUnit attackUnit)
    {
        _attackUnit = attackUnit;
    }

    public void Attack(ITarget target)
    {
        transform.LookAt(target.GetPosition());

        if (_isAttackCharged)
        {
            _attackUnit.Attack(target);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _isAttackCharged = false;
        yield return new WaitForSeconds(_reloadTime);
        _isAttackCharged = true;
    }
   

}
