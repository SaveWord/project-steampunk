using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.AntMoves
{
    [Serializable]
    public class Dasher
    {
        public bool IsDashCharged { get{ return _isDashCharged;} private set { } }

        [SerializeField] private float _dashMinCooldown = 2f;
        [SerializeField] private float _dashMaxCooldown = 10f;
        [SerializeField] private float _dashForce = 15f;

        private MonoBehaviour _dashDoer;
        private Rigidbody _rBody;
        private bool _isDashCharged = false;

        public Dasher(Rigidbody rBody, MonoBehaviour dashDoer)
        {
            _rBody = rBody;
            _dashDoer = dashDoer;

            ReloadDash();
        }

        public void Dash(Action DoOnStart)
        {
            DoOnStart();
            _rBody.AddForce(GetDashDirection().normalized * _dashForce, ForceMode.Impulse);
            ReloadDash();
        }

        private Vector3 GetDashDirection()
        {
            var direction = _rBody.transform.right * (Random.Range(0, 2) * 2 - 1);

            direction.y = 0.5f;
            return direction;
        }

        public void ReloadDash()
        {
            _dashDoer.StartCoroutine(ReloadDashCoroutine());
        }

        private IEnumerator ReloadDashCoroutine()
        {
            _isDashCharged = false;
            yield return new WaitForSeconds(Random.Range(_dashMinCooldown, _dashMaxCooldown));
            _isDashCharged = true;
        }
    }
}
