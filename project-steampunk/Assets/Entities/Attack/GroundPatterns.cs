using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies.Bullets
{
    public class GroundPatterns : MonoBehaviour
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;
        [SerializeField]  private float _speed ;
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _chargeTime;
        //[SerializeField] private bool FollowForSomeTime;

        private GameObject targetObject;
        //private float followDuration = 15f;
        // private Vector3 lastKnownPosition;
        public Material materialNone;
        [SerializeField]
        public Material materialCharge;
        [SerializeField]
        public Material materialDamage;

        private Rigidbody _rBody;
        private float _timeOnFly;

        private void Awake()
        {
            materialNone = this.GetComponent<MeshRenderer>().material ;
            //this.GetComponent<MeshRenderer>().material = null;
            NewPulseCycle();
        }

        void NewPulseCycle()
        {
            Debug.Log("new cycle");
            this.GetComponent<MeshRenderer>().material = materialCharge;
            StartCoroutine(ChargeCoroutine());
        }

        // Executes platform's attack
        private IEnumerator ChargeCoroutine()
        {
            Debug.Log("starting charge");
            this.GetComponent<MeshRenderer>().material = materialCharge;
            
            yield return new WaitForSeconds(_chargeTime);

            // changing color of platform to red
            this.GetComponent<MeshRenderer>().material = materialDamage;
            
            // waiting just a little for player to see red coloring
            yield return new WaitForSeconds(0.2f);
            // changing color back to basic state
            this.GetComponent<MeshRenderer>().material = materialCharge;
            yield return new WaitForSeconds(_chargeTime);
            this.GetComponent<MeshRenderer>().material = materialDamage;

            // waiting just a little for player to see red coloring
            yield return new WaitForSeconds(0.2f);
            this.GetComponent<MeshRenderer>().material = materialNone;
            StartCoroutine(CooldownCoroutine());
        }

        // Provides cooldown for platform attack cycle
        private IEnumerator CooldownCoroutine()
        {
            Debug.Log("waiting cooldown");
            yield return new WaitForSeconds(_cooldownTime);
            NewPulseCycle();
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
        }

        private void DealDamage(GameObject target)
        {
            //var damageable = target.GetComponent<IHealth>();
           // damageable.TakeDamage(_damage);
            target.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(_damage);
            Debug.Log("attack from pizza");
        }
    }
}

