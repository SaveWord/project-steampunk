using Enemies;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    [Header("Basics")]
    public int _attackQueue = 0;
    bool instanciated = false;
    private List<RangeAttack> _attaksList = new List<RangeAttack>();
    private List<AttackConstruct> _attaks = new List<AttackConstruct>();
   private Animator _animator;

    private bool _OnReload;

    private void Awake()
    {
        _animator = GameObject.FindGameObjectWithTag("animated").GetComponent<Animator>();
        _OnReload = true;
    }

    public void SetAttack(List<AttackConstruct> attack)
    {
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
        _attaksList[_attackQueue].Activate(target, attack[_attackQueue].patternSpawn);
        StartCoroutine(Reload(attack[_attackQueue].timeoutAfter));
        yield return new WaitForSeconds(Time);
        //_animator.SetBool(atkAnim, false);
       
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