using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected IAttackTarget _attackTarget;
        protected StateMachine _stateMachine;
    }
}

