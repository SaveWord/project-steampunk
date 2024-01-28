using Enemies.Attacks.Attacks;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected Stats _stats;
        protected StateMachine _stateMachine;
    }    
}

