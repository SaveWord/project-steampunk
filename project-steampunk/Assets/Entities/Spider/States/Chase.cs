using UnityEngine;
using UnityEngine.AI;

namespace Enemies.SpiderStates
{
    public class Chase : IState
    {
        private NavMeshAgent _nMeshAgent;
        private SpiderMover _spiderMover;
        private ITargetDetector _targetHolder;

        public Chase(NavMeshAgent nMeshAgent, SpiderMover spiderMover, ITargetDetector targetHolder)
        {
            _nMeshAgent = nMeshAgent;
            _spiderMover = spiderMover;
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
            {
                _spiderMover.MoveToTarget(target);
                //_spiderMover.Dash(target);
            }
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
