using Enemies.AntStates;
using System;
using UnityEngine;
using UnityEngine.AI;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;

namespace Enemies
{
    [RequireComponent(typeof(TargetAttacker), typeof(TargetDetector), typeof(AntMover))]
    public class Ant : Enemy
    {
        [SerializeField]
        public List<AttackConstruct> _attacksCollection;
        [SerializeField]
        private float _transitionCooldownTime;
        [SerializeField] 
        private bool _canChange = true;

        private void Awake()
        {
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<TargetAttacker>();
            var antMover = gameObject.GetComponent<AntMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var idle = new Idle();
            var chase = new Chase(navMeshAgent, antMover, targetDetector);
            var attack = new Attack(targetDetector, targetAttacker, antMover, _attacksCollection);
            var fightBack = new FightBack(targetDetector, targetAttacker, antMover);

            _stateMachine.AddTransition(idle, attack, TargetAvailable());
            _stateMachine.AddTransition(chase, attack, TargetAvailable());
            _stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            _stateMachine.AddTransition(idle, attack, AmIUnderAttack());
            _stateMachine.AddTransition(attack, idle, AmIUnderAtPeace());

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
        }

        private IEnumerator CooldownTransition()
        {
            _stateMachine.Tick();
            _canChange = false;
            yield return new WaitForSeconds(_transitionCooldownTime);
            _canChange = true;
        }

        private void Update()
        {
            if (_canChange)
            {
                
                StartCoroutine(CooldownTransition());
            }

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