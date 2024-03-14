using UnityEngine;

namespace Enemies.AntStates
{
    public class FightBack : IState
    {
        ITargetDetector _targetHolder;
        ITargetAttacker _targetAttacker;
        AntMover _antMover;

        public FightBack(ITargetDetector targetHolder, ITargetAttacker targetAttacker, AntMover antMover)
        {
            _targetHolder = targetHolder;
            _targetAttacker = targetAttacker;
            _antMover = antMover;
        }

        public Color GizmoColor()
        {
            return Color.red;
        }

        public void Tick()
        {
            var _target = _targetHolder.GetTarget();

            if (_target != null)
            {
                _antMover.Dash(_target);
                _targetAttacker.Attack(_target);
                //Debug.Log("fighting back mode");
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
