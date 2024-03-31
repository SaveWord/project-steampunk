using Enemies.Attacks.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public interface ITargetAttacker
    {
        public void Attack(ITarget target, List<AttackConstruct> attack);
    }
}
