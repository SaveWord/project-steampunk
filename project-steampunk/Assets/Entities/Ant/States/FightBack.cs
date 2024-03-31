using UnityEngine;
using Enemies.AntStates;
using System;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;

namespace Enemies.AntStates
{
    public class FightBack : IState
    {
        ITargetDetector _targetHolder;
        ITargetAttacker _targetAttacker;
        List<AttackConstruct> _attacksCollection;
        AntMover _antMover;

        public FightBack(ITargetDetector targetHolder, ITargetAttacker targetAttacker, AntMover antMover, List<AttackConstruct> attacksCollection)
        {
            _targetHolder = targetHolder;
            _targetAttacker = targetAttacker;
            _antMover = antMover;
            _attacksCollection = attacksCollection;
        }

        public Color GizmoColor()
        {
            return Color.cyan;
        }

        public void Tick()
        {
            var _target = _targetHolder.GetTarget();

            if (_target != null)
            {
                _antMover.Dash(_target);
                _targetAttacker.Attack(_target, _attacksCollection);
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
