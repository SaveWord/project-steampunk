using System;
using UnityEngine;
using Enemies.Bullets;
using System.Collections.Generic;

namespace Enemies.Attacks.Attacks
{
    [Serializable]
    public class AttackConstruct
    {
        public RangeAttack attack;
        public Transform patternSpawn;
        public float cooldown;
        public float startTime;
    }

}
