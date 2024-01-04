using UnityEngine;
using UnityEngine.AI;

namespace Enemies.AntStates
{
    public class Chase : IState
    {
        private NavMeshAgent _nMeshAgent;
        private AttackTargetDetector _aTargetDetector;

        public Chase(NavMeshAgent nMeshAgent, AttackTargetDetector targetDetector)
        {
            _nMeshAgent = nMeshAgent;
            _aTargetDetector = targetDetector;
        }

        public Color GizmoColor()
        {
            return Color.red;
        }

        public void OnEnter()
        {
            _nMeshAgent.enabled = true;
        }

        public void OnExit()
        {
            _nMeshAgent.enabled = false;
        }

        public void Tick()
        {
            var target = _aTargetDetector.GetTarget();

            if(target != null)
                _nMeshAgent.SetDestination(target.GetPosition());
        }
    }
}
