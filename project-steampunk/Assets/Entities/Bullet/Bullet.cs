using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Attacks
{
    public class Bullet : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<float> OnDestroy;

        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _speed;
        [SerializeField] private int _collisionReactionLayer;

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
            OnDestroy?.Invoke(_timeOnFly);
            OnDestroy.RemoveAllListeners();
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _collisionReactionLayer)
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

