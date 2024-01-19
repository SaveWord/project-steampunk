using UnityEngine;

namespace Enemies
{
    public class TargetDetector : MonoBehaviour, ITargetDetector
    {
        [SerializeField] private LayerMask _viewMask;
        [SerializeField] private float _visionRadius = 20f;

        private ITarget _target;
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.isTrigger = true;
            _collider.radius = _visionRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<ITarget>();

            if (target != null)
            {
                _target = target;
            }
        }

        public bool IsTargetAvailable()
        {
            if (_target != null && IsTargetVisible())
                return true;
            else 
                return false;
        }

        private bool IsTargetVisible()
        {
            if (_target != null && SphereCastAll(out RaycastHit hitInfo))
            {                
                if (hitInfo.collider.gameObject.GetInstanceID() == _target.GetTargetID())
                {
                    return true;
                }
            }
            return false;
        }

        private bool SphereCastAll(out RaycastHit hitInfo)
        {
            var hits = Physics.SphereCastAll(transform.position, 1f, _target.GetPosition() - transform.position, 100, ~_viewMask);

            Debug.DrawRay(transform.position, (_target.GetPosition() - transform.position) * 100, Color.yellow);

            if (hits.Length != 0) 
            {
                hitInfo = FindClosestHit(hits);
                return true;
            }

            hitInfo = default;
            return false;
        }

        private RaycastHit FindClosestHit(RaycastHit[] hits)
        {
            var closestHit = hits[0];

            for(int i = 1; i < hits.Length; i++)
            {
                if(Vector3.Distance(transform.position, hits[i].collider.transform.position) < 
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
    }
}