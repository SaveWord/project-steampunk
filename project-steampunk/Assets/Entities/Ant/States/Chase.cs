using UnityEngine;
using UnityEngine.AI;

namespace Enemies.AntStates
{
    public class Chase : IState
    {
        private NavMeshAgent _nMeshAgent;
        private AntMover _antMover;
        private ITargetDetector _targetHolder;      

        public Chase(NavMeshAgent nMeshAgent, AntMover antMover, ITargetDetector targetHolder)
        {
            _nMeshAgent = nMeshAgent;
            _antMover = antMover;
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
                _antMover.MoveToTarget(target);
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
