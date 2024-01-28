using Enemies;
using Enemies.Attacks.Attacks;
using System.Collections;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    [SerializeField] private RangeAttack _attack;
    [SerializeField] private Transform _attackSpot;
    [SerializeField] private float _reloadTime = 2f;

    private bool _OnReload;


    private void Awake()
    {
        _OnReload = true;
        _attack = Instantiate(_attack, transform);
    }

    public void SetAttack(RangeAttack attack)
    {
        _attack = attack;
    }

    public void Attack(ITarget target)
    {
        transform.LookAt(target.GetPosition());

        if (!_attack.Activated && _OnReload)
        {
            _attack.Activate(target, _attackSpot);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _OnReload = false;
        yield return new WaitForSeconds(_reloadTime);
        _OnReload = true;
    }
   

}
