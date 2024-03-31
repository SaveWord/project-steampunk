using UnityEngine;

namespace Enemies.BossStates
{
    public class Idle : IState
    {
        public Color GizmoColor()
        {
            return Color.gray;
        }

        public void OnEnter()
        {
            return;
        }

        public void OnExit()
        {
            return;
        }

        public void Tick()
        {
            Debug.Log("idle mode");
            return;
        }
    }
}

