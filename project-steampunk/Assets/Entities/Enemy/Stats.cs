using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class Stats
    {
        [SerializeField] private float _hp = 100;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _visionRadius = 20f;
        [SerializeField] private float _timeBeforeReposition = 2f;
        [SerializeField] private float _reloadTime = 2f;

        public float Hp { get { return _hp; } }
        public float MoveSpeed { get{ return _moveSpeed;} }
        public float VisionRadius { get { return _visionRadius; } }
        public float TimeBeforeReposition { get { return _timeBeforeReposition; } }
        public float ReloadTime { get { return _reloadTime; } }
    }
}