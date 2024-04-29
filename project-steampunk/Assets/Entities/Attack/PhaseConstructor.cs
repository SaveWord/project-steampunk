using System;
using UnityEngine;
using Enemies.Bullets;
using System.Collections.Generic;

namespace Enemies.Attacks.Attacks
{
    [Serializable]
    public class PhaseConstruct
    {
        public List<AttackConstruct> attacksCollection;

        [Header("% of health when it changes to next phase")]
        public int healthPercentageChangePhase;
        [SerializeField]
        public IBossTargetAttacker attacker;


    }

}
