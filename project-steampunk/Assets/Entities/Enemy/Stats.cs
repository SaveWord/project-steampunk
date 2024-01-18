using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class Stats
    {
        [SerializeField] private float _hp = 100;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _timeBeforeReposition = 2f;

        public float Hp { get { return _hp; } }
        public float MoveSpeed { get{ return _moveSpeed;} }
        public float TimeBeforeReposition { get { return _timeBeforeReposition; } }
    }
}