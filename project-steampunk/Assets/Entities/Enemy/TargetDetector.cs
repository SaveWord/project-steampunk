using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class TargetDetector : MonoBehaviour, ITargetDetector
    {
        [Header("Basics")]
        [SerializeField] private LayerMask _viewMask;
        [SerializeField] private float _detectionRadius = 20f;
        [SerializeField] private float _timeToForgets = 5f;

        [Header("SphereCast")]
        [SerializeField] private float _sphereCastRadius = 1f;
        [SerializeField] private float _sphereCastMaxDist;
        [SerializeField] private Transform _castPoint;

        private ITarget _target;
        private SphereCollider _collider;
        private controlarrow _controlarrow;
        private bool _TheyAreShootingMe = false;
        private IEnumerator _timerCoroutine;
        [SerializeField] private bool bee;
        [SerializeField] private bool InstantAgr;
        private void Awake()
        {
            if (_castPoint == null)
                _castPoint = transform;
            if(_sphereCastMaxDist == null)
                _sphereCastMaxDist = _detectionRadius;
            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.isTrigger = true;
            _collider.radius = _detectionRadius;
            _controlarrow = GetComponent<controlarrow>();
            _timerCoroutine = Forget();
            if (InstantAgr)
            {
                _target = GameObject.FindWithTag("Player").GetComponent<ITarget>();
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
            if (bee && _target != null)
                return true;
            if (!bee && _target != null && IsTargetVisible())
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
                    _controlarrow.Show();
                    _controlarrow.ChangeColorToRed();
                    return true;
                }
            }
            _controlarrow.Hide();
            return false;
        }


        private bool CastSphereToTarget(out RaycastHit closestHitInfo)
        {
            var hits = Physics.SphereCastAll(_castPoint.position, _sphereCastRadius,
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
                if (Vector3.Distance(_castPoint.position, hits[i].collider.transform.position) <
                Vector3.Distance(_castPoint.position, closestHit.collider.transform.position))
                    closestHit = hits[i];
            }
            return closestHit;
        }


        public ITarget GetTarget()
        {

            return _target;
        }


        public void GetShot()
        {
            _TheyAreShootingMe = true;
            //StopCoroutine(_timerCoroutine);
            //_timerCoroutine = Forget();
            //StartCoroutine(_timerCoroutine);
        }

        //TODO: forget that i was shot


        public bool AmIUnderAttack()
        {
            if (_TheyAreShootingMe)
            {
                _target = GameObject.FindWithTag("Player").GetComponent<ITarget>();
                if (_target == null)
                {
                    return false;
                }

                return true;
            }
            else
                return false;
        }


        IEnumerator Forget()
        {
            yield return new WaitForSeconds(_timeToForgets);
            _TheyAreShootingMe = false;
            _target = null;
        }
    }
}