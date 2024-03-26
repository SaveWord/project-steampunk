using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BossStates
{
    public class Chase : IState
    {
        private NavMeshAgent _nMeshAgent;
        private BossMover _bossMover;
        private ITargetDetector _targetHolder;      

        public Chase(NavMeshAgent nMeshAgent, BossMover antMover, ITargetDetector targetHolder)
        {
            _nMeshAgent = nMeshAgent;
            _bossMover = antMover;
            _targetHolder = targetHolder;
        }

        public Color GizmoColor()
        {
            return Color.yellow;
        }

        public void Tick()
        {
            var target = _targetHolder.GetTarget();

            if (target != null)
                _bossMover.MoveToTarget(target);
        }

        public void OnEnter()
        {
            return;
        }

        public void OnExit()
        {
            return;
        }

    }
}
