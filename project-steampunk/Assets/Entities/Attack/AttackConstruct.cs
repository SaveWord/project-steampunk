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
        public List<AttackBaseClass> attacks = new List<AttackBaseClass>();
        public Transform patternSpawn;
        public float timeoutAfter;
        public float startTime;
    }
   
}
