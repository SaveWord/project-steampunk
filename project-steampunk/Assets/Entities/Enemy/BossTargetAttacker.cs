using Enemies;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTargetAttacker : MonoBehaviour, IBossTargetAttacker
{
    [Header("Basics")]
    //private RangeAttack _attack;
    public int _attackQueue = 0;
    bool instanciated = false;
    private List<AttackBaseClass> _attaksList= new List<AttackBaseClass>(); 
    private Animator _animator;

    private bool _NoReload;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _NoReload = true;
    }

    public void SetAttack(List<AttackConstruct> attack)
    {
        instanciated = true;
        foreach (var atk in attack)
        {
            
                var tempObject = Instantiate(atk.attacks[0], atk.patternSpawn);
                tempObject.patternSpawnPoint = atk.patternSpawn;
                //tempObject.gameObject.SetActive(false);
                _attaksList.Add(tempObject);
            
        }
    }

    public void Attack(ITarget target, List<AttackConstruct> attack)
    {
        if(!instanciated)
            SetAttack(attack);

        if (_attackQueue == _attaksList.Count)
            _attackQueue = 0;

        if (_NoReload) //!_attack.Activated && 
        {
            _attaksList[_attackQueue].gameObject.SetActive(true);
            _attaksList[_attackQueue].Activate(target, _attaksList[_attackQueue].patternSpawnPoint);
            _animator.SetBool("isAttacking0", true);
            StartCoroutine(Reload(attack[_attackQueue].timeoutAfter));
        }       
    }

    private IEnumerator Reload(float reloadTime)
    {
        _NoReload = false;
        yield return new WaitForSeconds(reloadTime);
        _NoReload = true;
        _attackQueue++;
        //_animator.SetBool("isAttacking0", false);
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
