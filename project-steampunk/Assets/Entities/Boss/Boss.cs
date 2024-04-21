using Enemies.BossStates;
using System;
using UnityEngine;
using UnityEngine.AI;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;

namespace Enemies
{
    [RequireComponent(typeof(HpHandler), typeof(TargetDetector), typeof(BossMover))]
    public class Boss : Enemy
    {
        [SerializeField]
        private float _maxHp;

        [Header("% of health when it changes to next phase")]
        [SerializeField]
        private int healthPercentageChangePhase;
        [SerializeField] 
        public List<AttackConstruct> _attacksCollection;
        [SerializeField]
        public List<PhaseConstruct> _phasesCollection;
        private StateMachine _stateMachine;
        private List<Attack> _attackStatesCollection = new List<Attack>();
        private List<Func<bool>> _phasesConditions = new List<Func<bool>>();
        private Attack attackGround;
        [SerializeField] private bool bee = false;
        public int HpPercentage;

        //private Attack attackGround;
        private void Awake()
        {

            //TODO: clean мусор
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<BossTargetAttacker>();
            var arenaAttacker = gameObject.GetComponent<ArenaAttacker>();
            var bossMover = gameObject.GetComponent<BossMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var bossHealth = GetComponent<HpHandler>();
            bossHealth.MaxHp = _maxHp;

            GetComponentInParent<IHealth>().OnHPChanged += HandleHpPercentage;


            var idle = new Idle();
            var chase = new Chase(navMeshAgent, bossMover, targetDetector);
           // attack = new Attack(targetDetector, targetAttacker, bossMover, _attacksCollection);
            //attackGround = new Attack(targetDetector, arenaAttacker, bossMover, _attacksCollection);
           // var fightBack = new FightBack(targetDetector, targetAttacker, bossMover);

            //dectaring state for each element in list from editor
            var stateOrder = 0;


            /* foreach (PhaseConstruct phase in _phasesCollection)
             {
                 _attackStatesCollection.Add( new Attack(targetDetector, gameObject.AddComponent<BossTargetAttacker>() , bossMover, phase.attacksCollection));

                 if (_attacksCollection[0] != null && stateOrder == 0)
                 {
                     _stateMachine.AddTransition(idle, _attackStatesCollection[stateOrder], TargetAvailable());
                     _stateMachine.AddTransition(idle, _attackStatesCollection[stateOrder], AmIUnderAttack());
                 }
                 else
                     _stateMachine.AddTransition(_attackStatesCollection[stateOrder - 1], _attackStatesCollection[stateOrder], HealthCondition());

                 _stateMachine.AddTransition(_attackStatesCollection[stateOrder], idle, TargetNotAvailable());
                 stateOrder++;
             }*/
            if (bee)
            {
                attackGround = new Attack(targetDetector, targetAttacker, bossMover, _attacksCollection);

                _stateMachine.AddTransition(idle, attackGround, TargetAvailable());

                _stateMachine.AddTransition(attackGround, idle, TargetNotAvailable());
                _stateMachine.AddTransition(attackGround, idle, AmIUnderAtPeace());
            }
            else
            {
                foreach (PhaseConstruct phase in _phasesCollection)
                {
                    _attackStatesCollection.Add(new Attack(targetDetector, gameObject.AddComponent<BossTargetAttacker>(), bossMover, phase.attacksCollection));

                    if (_attackStatesCollection[0] != null && stateOrder == 0)
                    {
                        _stateMachine.AddTransition(idle, _attackStatesCollection[stateOrder], TargetAvailable());
                        _stateMachine.AddTransition(idle, _attackStatesCollection[stateOrder], AmIUnderAttack());

                    }
                    else
                    {
                        // _phasesConditions.Add(new Func<bool>(() => _phasesCollection[stateOrder-1].healthPercentageChangePhase > HpPercentage && HpPercentage > phase.healthPercentageChangePhase));

                        // _stateMachine.AddTransition(_attackStatesCollection[stateOrder - 1], _attackStatesCollection[stateOrder], _phasesConditions[stateOrder-1]);
                    }

                    _stateMachine.AddTransition(_attackStatesCollection[stateOrder], idle, TargetNotAvailable());
                    stateOrder++;
                }


                _stateMachine.AddTransition(_attackStatesCollection[0], _attackStatesCollection[1], HealthCondition1());
                _stateMachine.AddTransition(_attackStatesCollection[1], _attackStatesCollection[2], HealthCondition2());
                // _stateMachine.AddTransition(attackGround, _attackStatesCollection[0], ArenaFinished());
                Func<bool> HealthCondition1() => () => (_phasesCollection[0].healthPercentageChangePhase > HpPercentage) && (HpPercentage > _phasesCollection[1].healthPercentageChangePhase);
                Func<bool> HealthCondition2() => () => (_phasesCollection[1].healthPercentageChangePhase > HpPercentage) && (HpPercentage > _phasesCollection[2].healthPercentageChangePhase);
            }
            //_stateMachine.AddTransition(attack, attackGround, Health50());
            // _stateMachine.AddTransition(attackGround, attack, TargetAvailable());
            //was right it and idle invoked

            //_stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            // _stateMachine.AddTransition(attack, attackGround, TargetNotAvailable());
            // _stateMachine.AddTransition(attack, idle, AmIUnderAtPeace());
            Func<bool> HealthCondition() => () =>  90 > ((100 * bossHealth.CurrentHp) / bossHealth.MaxHp);

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
           // Func<bool> ArenaFinished() => () => !arenaAttacker.ONReload();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
            //InvokeRepeating("StartCoroutine(ChangeCoroutine)", 5f, 10f);
        }

        private void HandleHpPercentage(float currentHp)
        {
            HpPercentage = (int)((100 * currentHp) / _maxHp);
           // Debug.Log(_phasesConditions[0] +" ay");
           // Debug.Log(_phasesConditions[1]+ " ay");
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
        
        private IEnumerator ChangeCoroutine(Attack atk, Attack atk1)
        {
            yield return new WaitForSeconds(2f);
            _stateMachine.SetState(atk);
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
        //        Gizmos.color = _stateMachine.GetGizmoColor();
               // Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
            }
        }
    }
}



/*

using Enemies.BossStates;
using System;
using UnityEngine;
using UnityEngine.AI;
using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;

namespace Enemies
{
    [RequireComponent(typeof(HpHandler), typeof(TargetDetector), typeof(BossMover))]
    public class Boss : Enemy
    {
        [Header("% of health when it changes to next phase")]
        [SerializeField]
        private int healthPercentageChangePhase;
        [SerializeField]
        public List<AttackConstruct> _attacksCollection;
        [SerializeField]
        public List<PhaseConstruct> _phasesCollection;
        private StateMachine _stateMachine;
        private List<Attack> _attackStatesCollection = new List<Attack>();

        public int HpPercentage;
        private float _maxHp;
        //private Attack attackGround;
        private void Awake()
        {

            //TODO: clean мусор
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<BossTargetAttacker>();
            //var arenaAttacker = gameObject.GetComponent<ArenaAttacker>();
            var bossMover = gameObject.GetComponent<BossMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();
            GetComponentInParent<IHealth>().OnHPChanged += HandleHpPercentage;
            var bossHealth = GetComponent<HpHandler>();
            _maxHp = bossHealth.MaxHp;


            var idle = new Idle();
            var chase = new Chase(navMeshAgent, bossMover, targetDetector);
            // attack = new Attack(targetDetector, targetAttacker, bossMover, _attacksCollection);
            // attackGround = new Attack(targetDetector, arenaAttacker, bossMover, _attacksCollection);
            // var fightBack = new FightBack(targetDetector, targetAttacker, bossMover);

            //dectaring state for each element in list from editor
            var stateOrder = 0;
           
            foreach (PhaseConstruct phase in _phasesCollection)
            {
                _attackStatesCollection.Add(new Attack(targetDetector, gameObject.AddComponent<BossTargetAttacker>(), bossMover, phase.attacksCollection));


                if (_attacksCollection[0] != null && stateOrder == 0)
                {
                    _stateMachine.AddTransition(idle, _attackStatesCollection[0], TargetAvailable());
                    _stateMachine.AddTransition(idle, _attackStatesCollection[0], AmIUnderAttack());
                }
                _stateMachine.AddTransition(_attackStatesCollection[stateOrder], idle, TargetNotAvailable());
                stateOrder++;
            }

            _stateMachine.AddTransition(_attackStatesCollection[0], _attackStatesCollection[1], HealthCondition1());
            _stateMachine.AddTransition(_attackStatesCollection[1], _attackStatesCollection[2], HealthCondition2());
            //_stateMachine.AddTransition(attack, attackGround, Health50());
            // _stateMachine.AddTransition(attackGround, attack, TargetAvailable());
            //was right it and idle invoked

            //_stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            // _stateMachine.AddTransition(attack, attackGround, TargetNotAvailable());
            // _stateMachine.AddTransition(attack, idle, AmIUnderAtPeace());
            Func<bool> HealthCondition1() => () => bossHealth.HandlePercentage(90);
            Func<bool> HealthCondition2() => () => bossHealth.HandlePercentage(50);

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
            //InvokeRepeating("StartCoroutine(ChangeCoroutine)", 2f, 8f);
            //StartCoroutine(ChangeCoroutine(attack, attackGround));
        }

        private void HandleHpPercentage(float currentHp)
        {
            HpPercentage = (int)((100 * currentHp) / _maxHp);
        }
        private void Update()
        {
            _stateMachine.Tick();
        }
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                //  Gizmos.color = _stateMachine.GetGizmoColor();
                //  Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
            }
        }
    }
}

*/







/*using Enemies.AntStates;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(TargetAttacker), typeof(TargetDetector), typeof(AntMover))]
    public class Ant : Enemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();

            var targetDetector = gameObject.GetComponent<TargetDetector>();
            var targetAttacker = gameObject.GetComponent<TargetAttacker>();
            var antMover = gameObject.GetComponent<AntMover>();
            var navMeshAgent = GetComponent<NavMeshAgent>();

            var idle = new Idle();
            var chase = new Chase(navMeshAgent, antMover ,targetDetector);
            var attack = new Attack(targetDetector, targetAttacker, antMover);
            var fightBack = new FightBack(targetDetector, targetAttacker, antMover);

            _stateMachine.AddTransition(idle, attack, TargetAvailable());
            _stateMachine.AddTransition(chase, attack, TargetAvailable());
            _stateMachine.AddTransition(attack, chase, TargetNotAvailable());
            _stateMachine.AddTransition(idle, fightBack, AmIUnderAttack());
            _stateMachine.AddTransition(fightBack, idle, AmIUnderAtPeace());

            Func<bool> TargetAvailable() => () => targetDetector.IsTargetAvailable();
            Func<bool> TargetNotAvailable() => () => !targetDetector.IsTargetAvailable();
            Func<bool> AmIUnderAttack() => () => targetDetector.AmIUnderAttack();
            Func<bool> AmIUnderAtPeace() => () => !targetDetector.AmIUnderAttack();

            _stateMachine.SetState(idle);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = _stateMachine.GetGizmoColor();
                Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
            }
        }
    }
}
*/