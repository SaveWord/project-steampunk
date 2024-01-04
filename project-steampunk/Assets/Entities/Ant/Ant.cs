using UnityEngine;

namespace Enemies
{
    public class Ant : Enemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();
        }
    }
}
