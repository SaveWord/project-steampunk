using UnityEngine;
using Enemies.Bullets;
using System.Collections.Generic;

namespace Enemies.Attacks.Attacks
{
    public abstract class Attack : MonoBehaviour
    {
        public abstract void Activate(ITarget target, Vector3 commonSpot);        
    }
}
