using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class TargetDetector : MonoBehaviour, ITargetDetector
    {
        [Header("Detector params")]
        [SerializeField] 
        private LayerMask _viewMask;
        [SerializeField] 
        private float _detectionRadius = 20f;
        [SerializeField] 
        private float _timeToForgets = 60f;
        [SerializeField] 
        private float _sphereCastMaxDist;
        [SerializeField] 
        private Transform _castPoint;

        [Header("Target params")]
        private ITarget _target;
        private SphereCollider _thisEnemyCollider;
        private controlarrow _enemyArrow;
        private IEnumerator _forgetTimerCoroutine;

        [Header("Enemy behavior switches")]
        [SerializeField]
        private bool _isShotByPlayer = false;
        [SerializeField] 
        private bool _isBee;
        [SerializeField] 
        private bool _isInstantlyAggred;

        private void Awake()
        {
            if (_castPoint == null)
                _castPoint = transform;
            if (_sphereCastMaxDist == null)
                _sphereCastMaxDist = _detectionRadius;

            _thisEnemyCollider = gameObject.AddComponent<SphereCollider>();
            _thisEnemyCollider.isTrigger = true;
            _thisEnemyCollider.radius = _detectionRadius;

            _enemyArrow = GetComponent<controlarrow>();

            _forgetTimerCoroutine = Forget();
            if (_isInstantlyAggred)
            {
                GetShot();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var target = other.GetComponent<ITarget>();

            if (target != null)
            {
                transform.LookAt(new Vector3(target.GetPosition().x, transform.position.y, target.GetPosition().z));
                _target = target;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var target = other.GetComponent<ITarget>();

            if (target != null)
            {
                _target = null;
            }
        }

        public bool IsTargetAvailable()
        {
            if (_isBee && _target != null)
                return true;
            if (!_isBee && _target != null)// && IsTargetVisible())
                return true;
            else
                return false;
        }

        private bool IsTargetVisible()
        {
            if (_target != null && CastSphereToTarget(out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetInstanceID() == _target.GetTargetID())
                {
                    _enemyArrow.Show();
                    _enemyArrow.ChangeColorToRed();
                    return true;
                }
            }
            _enemyArrow.Hide();
            return false;
        }

        public bool AmIUnderAttack()
        {
            if (_isShotByPlayer)
            {
                _target = GameObject.FindWithTag("Player").GetComponent<ITarget>();
                if (_target == null)
                    return false;

                return true;
            }
            else
                return false;
        }

        private bool CastSphereToTarget(out RaycastHit closestHitInfo)
        {
            var hits = Physics.SphereCastAll(_castPoint.position, 2f,
            _target.GetPosition() - _castPoint.position, _sphereCastMaxDist, ~_viewMask);
            Debug.DrawRay(_castPoint.position,
            _target.GetPosition() - _castPoint.position, Color.yellow);
            if (hits.Length != 0)
            {
                closestHitInfo = FindClosestHit(hits);
                return true;
            }
            closestHitInfo = default;
            return false;
        }

        private RaycastHit FindClosestHit(RaycastHit[] hits)
        {
            var closestHit = hits[0];

            for (int i = 1; i < hits.Length; i++)
            {
                if (Vector3.Distance(transform.position, hits[i].collider.transform.position) <
                   Vector3.Distance(transform.position, closestHit.collider.transform.position))
                {
                    closestHit = hits[i];
                }
            }
            return closestHit;
        }

        public ITarget GetTarget()
        {
            return _target;
        }

        public void GetShot()
        {
            _isShotByPlayer = true;
            StopCoroutine(_forgetTimerCoroutine);
            _forgetTimerCoroutine = Forget();
            StartCoroutine(_forgetTimerCoroutine);
        }

        IEnumerator Forget()
        {
            yield return new WaitForSeconds(_timeToForgets);
            _isShotByPlayer = false;
            _target = null;
        }
    }
}