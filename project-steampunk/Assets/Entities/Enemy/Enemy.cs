using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected EnemyStats _enemyStats;
        protected StateMachine _stateMachine;
    }    
}

