using Enemies.Attacks.AttackUnits;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected Stats _enemyStats;
        protected StateMachine _stateMachine;
    }    
}

