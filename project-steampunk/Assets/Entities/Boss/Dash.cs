using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.BossMoves
{
    [Serializable]
    public class Dash
    {
        public bool IsDashCharged { get{ return _isDashCharged;} private set { } }

        [SerializeField] private float _dashMinCooldown = 2f;
        [SerializeField] private float _dashMaxCooldown = 10f;
        [SerializeField] private float _dashForce = 15f;      
        [Range(0,1)][SerializeField] private float _dashHeight = 0.5f;

        private MonoBehaviour _actor;
        private Rigidbody _actorRBody;
        private bool _isDashCharged = false;

        public Dash(Rigidbody _actorRBody, MonoBehaviour _actor)
        {
            this._actorRBody = _actorRBody;
            this._actor = _actor;

            ReloadDash();
        }

        public void MakeDash(Action DoBeforeDash)
        {
            DoBeforeDash();
            _actorRBody.AddForce(GetDashDirection().normalized * _dashForce, ForceMode.Impulse);
            ReloadDash();
        }

        private Vector3 GetDashDirection()
        {
            var direction = _actorRBody.transform.right * (Random.Range(0, 2) * 2 - 1);

            direction.y = _dashHeight;
            return direction;
        }

        public void ReloadDash()
        {
            _actor.StartCoroutine(ReloadDashCoroutine());
        }

        private IEnumerator ReloadDashCoroutine()
        {
            _isDashCharged = false;
            yield return new WaitForSeconds(Random.Range(_dashMinCooldown, _dashMaxCooldown));
            _isDashCharged = true;
        }
    }
}
