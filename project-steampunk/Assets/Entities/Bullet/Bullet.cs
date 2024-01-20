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
            if (collision.gameObject.GetInstanceID() == Target.GetTargetID())
            {
                var damageable = collision.gameObject.GetComponent<IDamageable>();
                damageable.GetDamage(_damage);
            }

            SelfDestroy();
        }

        public void StartFly(Vector3 flyEndPoint)
        {
            transform.LookAt(flyEndPoint);
            _rBody.velocity = transform.forward * _speed;
        }
    }
}

