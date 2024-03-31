using UnityEngine;

namespace Enemies.Bullets
{
    public class Wall : Bullet
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField]  private float _speedWall;
        //[SerializeField] private bool FollowForSomeTime;

        private GameObject targetObject;
        private Vector3 _direction;
        private Vector3 lastKnownPosition;
        private Vector3 continueDirection;
        protected void Awake()
        {
            Physics.IgnoreLayerCollision(9, 9, true);
            Physics.IgnoreLayerCollision(6, 9, true);
           
        }
        private void Start()
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");
            transform.rotation = Quaternion.Euler(0, 90, 0);

           // _direction = (targetObject.position - transform.position).normalized;
        }
        protected void Update()
        {
            OnFly();
        }

        public virtual void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            if (_timeOnFly >= _lifeTime) SelfDestroy();

            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position+transform.forward, 1 * _speedWall);

        }

        protected void SelfDestroy()
        {
            Debug.Log("Die");
            Destroy(gameObject);
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                IHealth damageScript = collision.gameObject.GetComponent<IHealth>();
                if (damageScript != null)
                {
                    Debug.Log("hit player");
                    damageScript.TakeDamage(_damage);
                    SelfDestroy();
                }
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
            // _rBody.velocity = transform.forward * _speed;
        }

    }
}

