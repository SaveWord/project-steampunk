using UnityEngine;
using UnityEngine.AI;

namespace Enemies.AntStates
{
    public class Chase : IState
    {
        private NavMeshAgent _nMeshAgent;
        private ITargetHolder _targetHolder;

        public Chase(NavMeshAgent nMeshAgent, ITargetHolder targetHolder)
        {
            _nMeshAgent = nMeshAgent;
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
                _nMeshAgent.SetDestination(target.GetPosition());
        }

        public void OnEnter()
        {
            _nMeshAgent.enabled = true;
        }

        public void OnExit()
        {
            _nMeshAgent.enabled = false;
        }

    }
}
