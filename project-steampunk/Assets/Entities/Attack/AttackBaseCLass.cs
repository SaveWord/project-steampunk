using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Attacks.Attacks
{
    public abstract class AttackBaseClass : MonoBehaviour
    {
        public Transform patternSpawnPoint;
        public virtual bool Activated { get; private set; }

        public virtual void Activate(ITarget target, Transform attackSpot)
        {

        }
    }

}