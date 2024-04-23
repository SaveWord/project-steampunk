using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Attacks.Attacks
{
    public abstract class AttackBaseClass : MonoBehaviour
    {
        public Transform patternSpawnPoint;

        public virtual void Activate(ITarget target, Transform attackSpot)
        {

        }
    }

}