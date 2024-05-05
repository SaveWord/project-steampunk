using System.Collections;
using UnityEngine;

namespace Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] protected float _damage;
        [SerializeField] protected float _lifeTime;
        [SerializeField] private float BulletSpeed;
        protected float _speed;
        //[SerializeField] private bool FollowForSomeTime;

        protected GameObject targetObject; 
       
        protected Vector3 lastKnownPosition;
        protected Vector3 continueDirection;
        protected Rigidbody _rBody;
        protected float _timeOnFly = 4f;

        [Header("VFX")]
        [SerializeField] protected GameObject sphereDie;
        [SerializeField] protected float coroutineTimeDie;

        protected void Awake()
        {
            Physics.IgnoreLayerCollision(9, 9, true);
            Physics.IgnoreLayerCollision(6, 9, true);
            _timeOnFly = 0;
            _rBody = GetComponent<Rigidbody>();
            targetObject = GameObject.FindGameObjectWithTag("Player");
            if (targetObject != null)
            {
               
                _speed = 40 * BulletSpeed;
                lastKnownPosition = targetObject.GetComponent<ITarget>().GetPosition();
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
            AudioManager.InstanceAudio.PlaySfxEnemy("BulletDestroyed");
            Debug.Log("Die");
            StartCoroutine(SelfDestroyCoroutine());
        }
        protected IEnumerator SelfDestroyCoroutine()
        {
            sphereDie.SetActive(true);
            yield return new WaitForSeconds(coroutineTimeDie);
            Destroy(gameObject);
        }

        protected void OnTriggerEnter(Collider collision)
        {

            IHealth damageScript = collision.gameObject.GetComponent<IHealth>(); 
            if (damageScript != null && collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
            {
                Debug.Log("hit player");
                damageScript.TakeDamage(_damage);
                SelfDestroy();
            }
            if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground")|| collision.gameObject.layer == LayerMask.NameToLayer("Props"))&& collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
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

