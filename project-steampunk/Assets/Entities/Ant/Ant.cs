using Enemies.AntStates;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(TargetAttacker), typeof(TargetDetector))]
    public class Ant : Enemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();

            var _targetDetector = gameObject.GetComponent<TargetDetector>();
            var _targetAttacker = gameObject.GetComponent<TargetAttacker>();
            var _navMeshAgent = GetComponent<NavMeshAgent>();

            var idle = new Idle();
            var chase = new Chase(_navMeshAgent, _targetDetector);
            var attack = new Attack(_targetDetector, _targetAttacker);

            _stateMachine.AddTransition(idle, attack, TargetAvailable());
            _stateMachine.AddTransition(chase, attack, TargetAvailable());
            _stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            _stateMachine.AddTransition(attack, chase, ShouldChangePosition());

            Func<bool> TargetAvailable() => () => _targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !_targetDetector.IsTargetAvailable();
            Func<bool> ShouldChangePosition() => () => _targetAttacker.ShouldChangePosition();

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
