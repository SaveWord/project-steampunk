using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enemies.Attacks.Attacks;
namespace Enemies.AntStates
{
    public class Attack : IState
    {
        ITargetDetector _targetHolder;
        ITargetAttacker _targetAttacker;
        AntMover _antMover;
        List<AttackConstruct> _attacksCollection;
        private float time = 0f;


        public Attack(ITargetDetector targetHolder, ITargetAttacker targetAttacker, AntMover antMover, List<AttackConstruct> attacksCollection)
        {
            _targetHolder = targetHolder;
            _targetAttacker = targetAttacker;
            _antMover = antMover;
            _attacksCollection = attacksCollection;
        }

        public Color GizmoColor()
        {
            return Color.red;
        }

        public void Tick()
        {
            time += Time.deltaTime;
            Debug.Log("jhjgfggfhjk" + time);
            var _target = _targetHolder.GetTarget();

            if (time > 0.1f)
            {
                
                _antMover.SideStep(_target);
            }else if(time > 0.12f)
            {
                //_antMover.stop();
                time = 0;
            }
            

            if (_target != null)
            {
                //_antMover.Dash();
                _targetAttacker.Attack(_target , _attacksCollection);
                Debug.Log("fight mode");
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
/*using UnityEngine;

namespace Enemies.AntStates
{
    public class Attack : IState
    {
        ITargetDetector _targetHolder;
        ITargetAttacker _targetAttacker;
        AntMover _antMover;


       

        public Attack(ITargetDetector targetHolder, ITargetAttacker targetAttacker, AntMover antMover)
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
                Debug.Log("fight mode");
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
}*/