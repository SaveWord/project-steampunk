using UnityEngine;

namespace Enemies.BossStates
{
    public class FightBack : IState
    {
        ITargetDetector _targetHolder;
        IBossTargetAttacker _targetAttacker;
        BossMover _antMover;

        public FightBack(ITargetDetector targetHolder, IBossTargetAttacker targetAttacker, BossMover antMover)
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
                _antMover.Dash();
               // _targetAttacker.Attack(_target);
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
