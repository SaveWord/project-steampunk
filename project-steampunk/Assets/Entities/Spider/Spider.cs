using Enemies.SpiderStates;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(TargetAttacker), typeof(TargetDetector), typeof(SpiderMover))]
    public class Spider : Enemy
    {
        [SerializeField]
        private bool _canChange = true;

        private void Awake()
        {
            _stateMachine = new StateMachine();

            GetComponentInParent<IHealth>().OnDied += HandleDie;

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var spiderMover = gameObject.GetComponent<SpiderMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var chase = new Chase(navMeshAgent, spiderMover, targetDetector);

            _stateMachine.SetState(chase);
        }

        private void HandleDie()
        {
            _canChange = false;
        }

        private void Update()
        {
            if(_canChange)
                _stateMachine.Tick();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = _stateMachine.GetGizmoColor();
                Gizmos.DrawSphere(transform.position + Vector3.up * 6, 2f);
            }
        }
    }
}
