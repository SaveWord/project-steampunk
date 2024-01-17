using UnityEngine;

namespace Enemies
{
    public class TargetDetector : MonoBehaviour, ITargetDetector
    {
        [SerializeField] private LayerMask _viewMask;

        private ITarget _target;
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.isTrigger = true;
            _collider.radius = 20f;
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
            if (_target != null && Physics.Linecast(transform.position, _target.GetPosition(), out RaycastHit hitInfo, ~_viewMask))
            {                
                if (hitInfo.collider.gameObject.GetInstanceID() == _target.GetTargetID())
                {
                    Debug.DrawRay(transform.position, (_target.GetPosition() - transform.position) * hitInfo.distance, Color.yellow);
                    return true;
                }
            }
            return false;
        }

        public ITarget GetTarget()
        {
            return _target;
        }
    }
}