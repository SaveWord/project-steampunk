using UnityEngine;

namespace Enemies.Bullets
{
    public class Ring : Bullet
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;
        [SerializeField]  private float _speed ;
        //[SerializeField] private bool FollowForSomeTime;

        private GameObject targetObject;
        //private float followDuration = 15f;
        // private Vector3 lastKnownPosition;

        private Rigidbody _rBody;
        private float _timeOnFly;

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
           // targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            OnFly();
        }

        private void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            gameObject.transform.localScale += new Vector3(1*_speed, 0, 1* _speed);

            if (_timeOnFly >= _lifeTime) SelfDestroy();
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.GetInstanceID());
            if (collision.gameObject.GetInstanceID() == Target.GetTargetID())
            {
                DealDamage(collision.gameObject);
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")|| collision.gameObject.layer == LayerMask.NameToLayer("Props"))
            {
                SelfDestroy();
            }
            
        }

    }
}

