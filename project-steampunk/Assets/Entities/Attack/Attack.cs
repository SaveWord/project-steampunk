using UnityEngine;
using Enemies.Bullets;
using System.Collections.Generic;

namespace Enemies.Attacks.Attacks
{
    public interface IAttack
    {
        public void Activate(ITarget target, Transform attackSpot);
    }
}
