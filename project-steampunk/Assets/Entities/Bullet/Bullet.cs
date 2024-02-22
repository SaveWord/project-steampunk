using UnityEngine;

namespace Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _speed;
        [SerializeField] private bool FollowForSomeTime;

        private GameObject targetObject; 
        private float followDuration = 15f;
        private Vector3 lastKnownPosition;

        private Rigidbody _rBody;
        private float _timeOnFly;

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
            targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            OnFly();
        }

        private void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            if (_timeOnFly >= _lifeTime) SelfDestroy();
            if (targetObject != null && FollowForSomeTime)
            {
                if (_timeOnFly < followDuration)
                {
                    Vector3 targetDirection = (targetObject.transform.position - transform.position).normalized;
                    transform.position += targetDirection * _speed * Time.deltaTime;
                    lastKnownPosition = targetObject.transform.position;
                }
                else
                {
                    Vector3 continueDirection = (lastKnownPosition - transform.position).normalized;
                    transform.position += continueDirection * _speed * Time.deltaTime;
                }
            }
            else
            {
               Debug.LogWarning("Target object not found!");
            }
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.GetInstanceID());
            if (collision.gameObject.GetInstanceID() == Target.GetTargetID())
            {
                DealDamage(collision.gameObject);
            }
            SelfDestroy();
        }

        private void DealDamage(GameObject target)
        {
            //var damageable = target.GetComponent<IHealth>();
           // damageable.TakeDamage(_damage);
            target.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(_damage);
            Debug.Log("attack from bullet");
        }

        public void StartFly(Vector3 direction)
        {
            transform.LookAt(direction);
            _rBody.velocity = transform.forward * _speed;
        }
    }
}

