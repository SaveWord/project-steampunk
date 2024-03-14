using UnityEngine;

namespace Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] protected float _damage;
        [SerializeField] protected float _lifeTime;
        protected float _speed;
        //[SerializeField] private bool FollowForSomeTime;

        protected GameObject targetObject; 
       
        protected Vector3 lastKnownPosition;
        protected Vector3 continueDirection;
        protected Rigidbody _rBody;
        protected float _timeOnFly;

        protected void Awake()
        {
            _timeOnFly = 0;
            _rBody = GetComponent<Rigidbody>();
            targetObject = GameObject.FindGameObjectWithTag("Player");
            if (targetObject != null)
            {
                _speed = targetObject.GetComponent<PlayerMove>().GetSpeed() * 0.8f;
                lastKnownPosition = targetObject.transform.position;
                continueDirection = (lastKnownPosition - transform.position).normalized;
            }
            else
            {
                Debug.LogWarning("Target object not found!");
            }
        }

        protected void Update()
        {
            OnFly();
        }

        public virtual void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            if (_timeOnFly >= _lifeTime) SelfDestroy();
            
        }

        protected void SelfDestroy()
        {
            Debug.Log("Die");
            Destroy(gameObject);
        }

        protected void OnCollisionEnter(Collision collision)
        {
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

        protected void DealDamage(GameObject target)
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

