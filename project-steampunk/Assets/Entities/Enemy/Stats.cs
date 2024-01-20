using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class Stats
    {
        [SerializeField] private float _hp = 100;
        [SerializeField] private float _timeBeforeReposition = 2f;

        public float Hp { get { return _hp; } }
        public float TimeBeforeReposition { get { return _timeBeforeReposition; } }
    }
}