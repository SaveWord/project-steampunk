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

        private Rigidbody _rBody;
        private float _timeOnFly;

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            OnFly();
        }

        private void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            if (_timeOnFly >= _lifeTime)
                SelfDestroy();
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

