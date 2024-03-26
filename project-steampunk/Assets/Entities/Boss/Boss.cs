using Enemies.BossStates;
using System;
using UnityEngine;
using UnityEngine.AI;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;

namespace Enemies
{
    [RequireComponent(typeof(BossTargetAttacker), typeof(TargetDetector), typeof(BossMover))]
    public class Boss : Enemy
    {
        [SerializeField] 
        public List<AttackConstruct> _attacksCollection;
        [SerializeField] 
        private float phaseTimer = 4f;
        private bool change=false;
        private int _stateOrder=0;
        private StateMachine _stateMachine;
        private Attack attack;
        private Attack attackGround;
        private void Awake()
        {
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<BossTargetAttacker>();
            var arenaAttacker = gameObject.GetComponent<ArenaAttacker>();
            var bossMover = gameObject.GetComponent<BossMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var idle = new Idle();
            var chase = new Chase(navMeshAgent, bossMover, targetDetector);
            attack = new Attack(targetDetector, targetAttacker, bossMover, _attacksCollection);
            attackGround = new Attack(targetDetector, arenaAttacker, bossMover, _attacksCollection);
            var fightBack = new FightBack(targetDetector, targetAttacker, bossMover);

            _stateMachine.AddTransition(idle, attack, TargetAvailable());

           // _stateMachine.AddTransition(attack, attackGround, TargetNotAvailable());
           // _stateMachine.AddTransition(attackGround, attack, TargetAvailable());
           //was right it and idle invoked

            //_stateMachine.AddTransition(attack, chase, TargetNotAvailable());
           // _stateMachine.AddTransition(attack, attackGround, TargetNotAvailable());
           // _stateMachine.AddTransition(idle, attack, AmIUnderAttack());
            // _stateMachine.AddTransition(attack, idle, AmIUnderAtPeace());

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
            //InvokeRepeating("StartCoroutine(ChangeCoroutine)", 2f, 8f);
            StartCoroutine(ChangeCoroutine(attack, attackGround));
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private IEnumerator ChangeCoroutine(Attack atk, Attack atk1)
        {
            yield return new WaitForSeconds(2f);
            _stateMachine.SetState(atk);
            StartCoroutine(ChangeCoroutine(atk1, atk));
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = _stateMachine.GetGizmoColor();
                Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
            }
        }
    }
}


/*using Enemies.AntStates;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(TargetAttacker), typeof(TargetDetector), typeof(AntMover))]
    public class Ant : Enemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<TargetAttacker>();
            var antMover = gameObject.GetComponent<AntMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var idle = new Idle();
            var chase = new Chase(navMeshAgent, antMover ,targetDetector);
            var attack = new Attack(targetDetector, targetAttacker, antMover);
            var fightBack = new FightBack(targetDetector, targetAttacker, antMover);

            _stateMachine.AddTransition(idle, attack, TargetAvailable());
            _stateMachine.AddTransition(chase, attack, TargetAvailable());
            _stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            _stateMachine.AddTransition(idle, fightBack, AmIUnderAttack());
            _stateMachine.AddTransition(fightBack, idle, AmIUnderAtPeace());

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = _stateMachine.GetGizmoColor();
                Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
            }
        }
    }
}
*/