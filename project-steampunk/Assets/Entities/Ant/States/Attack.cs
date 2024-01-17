using UnityEngine;

namespace Enemies.AntStates
{
    public class Attack : IState
    {
        ITargetDetector _targetHolder;
        ITargetAttacker _targetAttacker;

        public Attack(ITargetDetector targetHolder, ITargetAttacker targetAttacker)
        {
            _targetHolder = targetHolder;
            _targetAttacker = targetAttacker;
        }

        public Color GizmoColor()
        {
            return Color.red;
        }

        public void Tick()
        {
            var _target = _targetHolder.GetTarget();

            if (_target != null)
                _targetAttacker.Attack(_target);
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
