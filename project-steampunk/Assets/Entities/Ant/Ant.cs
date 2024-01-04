using Enemies.AntStates;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Ant : Enemy
    {
        private void Awake()
        {
            var _aTargetDetector = gameObject.AddComponent<AttackTargetDetector>();
            var _navMeshAgent = GetComponent<NavMeshAgent>();

            _stateMachine = new StateMachine();

            var idle = new Idle();
            var chase = new Chase(_navMeshAgent, _aTargetDetector);

            _stateMachine.AddTransition(idle, chase, TargetAvailable());
            _stateMachine.AddTransition(chase, idle, TargetNotAvailable());

            _stateMachine.SetState(idle);

            Func<bool> TargetAvailable() => () => _aTargetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !_aTargetDetector.IsTargetAvailable();
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
    }
}
