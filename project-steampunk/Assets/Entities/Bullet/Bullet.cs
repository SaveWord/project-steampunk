using UnityEngine;

namespace Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;
        private float _speed;
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

            _speed = targetObject.GetComponent<PlayerMove>().GetSpeed() * 0.8f;
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
            else if (targetObject != null && !FollowForSomeTime)
            {
                lastKnownPosition = targetObject.transform.position;
                Vector3 continueDirection = (lastKnownPosition - transform.position).normalized;
                transform.position += continueDirection * _speed * Time.deltaTime;
            }
            else
            {
               Debug.LogWarning("Target object not found!");
            }
        }

        private void SelfDestroy()
        {
            Debug.Log("Die");
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("777777777777777777777777777777777777777777777777777777777777");
            player_health damageScript = collision.gameObject.GetComponent<player_health>();
            if (damageScript != null)
            {
                Debug.Log("hit player");
                damageScript.TakeDamage(_damage);
                SelfDestroy();
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")|| collision.gameObject.layer == LayerMask.NameToLayer("Props"))
            {
                SelfDestroy();
            }
            
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

