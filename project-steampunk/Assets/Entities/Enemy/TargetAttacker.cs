using Enemies;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    //private RangeAttack _attack;
    public int _attackQueue = 0;
    bool instanciated = false;
    private List<RangeAttack> _attaksList = new List<RangeAttack>();
    private List<AttackConstruct> _attaks = new List<AttackConstruct>();
    private Animator _animator;

    private bool _OnReload;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _OnReload = true;

    }

    public void SetAttack(List<AttackConstruct> attack)
    {
        //_attaks = attack;
        foreach (var atk in attack)
        {
            var tempObject = Instantiate(atk.attack, transform);
            _attaksList.Add(tempObject);
        }
        instanciated = true;
    }

    public void Attack(ITarget target, List<AttackConstruct> attack)
    {
        if(!instanciated)
            SetAttack(attack);

        transform.LookAt(new Vector3(target.GetPosition().x, transform.position.y, target.GetPosition().z));

        if (_attackQueue == attack.Count)
            _attackQueue = 0;

        if (_OnReload) //!_attack.Activated && 
        {

            StartCoroutine(StartAttack(attack[_attackQueue].startTime, attack, target));
            
        }
    }

    private IEnumerator StartAttack(float Time, List<AttackConstruct> attack, ITarget target)
    {
        var atkAnim = "isAttacking" + _attackQueue;
        _animator.SetBool(atkAnim, true);
        yield return new WaitForSeconds(Time);
        _animator.SetBool(atkAnim, false);
        _attaksList[_attackQueue].Activate(target, attack[_attackQueue].patternSpawn);
        StartCoroutine(Reload(attack[_attackQueue].timeoutAfter));
    }
    private IEnumerator Reload(float reloadTime)
    {
        _OnReload = false;
        yield return new WaitForSeconds(reloadTime);
        _OnReload = true;
        _attackQueue++;
    }

    private IEnumerator DestroyBullet(float time, RangeAttack attack)
    {

        yield return new WaitForSeconds(time);
        Destroy(attack);
        // _animator.SetBool("isAttacking", false);
    }
}

/*public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    [SerializeField] private RangeAttack _attack;
    [SerializeField] private RangeAttack _attack2;
    [SerializeField] private RingAttack _attack1;
    [SerializeField] private Transform _attackSpot;
    [SerializeField] private Transform _attackSpot1;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private float _reloadTime1 = 10f;
    private Animator _animator;
    int count = 1;
    private bool _OnReload;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _OnReload = true;
        _attack = Instantiate(_attack, transform);
        _attack1 = Instantiate(_attack1, transform);
        _attack2 = Instantiate(_attack2, transform);
        InvokeRepeating("Attack1", 2, _reloadTime1);
        InvokeRepeating("Attack1", 3, _reloadTime1);
    }

    public void SetAttack()
    {

    }

    public void Attack(ITarget target)
    {
        transform.LookAt(target.GetPosition());

        if (!_attack.Activated && _OnReload)
        {
            if (count < 3)
            { _attack.Activate(target, _attackSpot); count += 1; }
            else if (count == 3)
            { _attack2.Activate(target, _attackSpot); count = 1; }

            //_animator.SetBool("isAttacking", true);
            StartCoroutine(Reload(_reloadTime));
        }
    }
    public void Attack1()
    {
        _attack1.Activate(null, _attackSpot1);
        //_animator.SetBool("isAttacking", true);

    }
    private IEnumerator Reload(float reloadTime)
    {
        _OnReload = false;
        yield return new WaitForSeconds(reloadTime);
        _OnReload = true;
        // _animator.SetBool("isAttacking", false);
    }


}*/
