using UnityEngine;

public class AttackTargetDetector : MonoBehaviour
{
    private IAttackTarget _target;
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = gameObject.AddComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IAttackTarget>();

        if (target != null)
        {
            _target = target;
        }
    }

    public void SetDetectionRadius(float radius)
    {
        _collider.radius = radius;
    }

    public bool IsTargetAvailable()
    {
        if (_target == null || Vector3.Distance(_target.GetPosition(), transform.position) < _collider.radius)
            return true;
        else 
            return false;
    }

    public IAttackTarget GetTarget()
    {
        return _target;
    }
}
