using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Attacks.Attacks
{
    public class LaserAttack : AttackBaseClass
    {
        public bool Activated { get; set; }

        /*
        // Update is called once per frame
        void Update()
        {
            Vector3 direction = Camera.main.transform.position - transform.position;
            Quaternion bulletRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = bulletRotation;
        }

        protected void DealDamage(GameObject target)
        {
            target.TryGetComponent(out IHealth damageable);
            damageable?.TakeDamage(10);
            Debug.Log("attack from ll ");
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                DealDamage(collision.gameObject);
            }
        }
        */
        [Header("Attack Parametres")]
        public Transform patternSpawnPoint;
        public Camera _target;

        public int _followDistance;
        public float _damage = 1;
        public float _attackDuration = 4f;
        public float _chargeDuration = 1.3f;
        
        [SerializeField]
        private float _laserCooldown;

        [SerializeField] private LayerMask transparentLayer;

        private bool _OnReload = false;

        private bool _isAttacking = false;
        private Queue<Vector3> _storedPositions = new Queue<Vector3>();
        private Vector3 _lastPos;

        private LaserAttack _laser;

        [Header("Visual Parametres")]

        [SerializeField]
        private Material _chargeMat;
        [SerializeField]
        private Material _targetMat;
        private ParticleSystem _particle;

        private bool _damageCooldown = false;
        void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
            gameObject.SetActive(false);
            _target = Camera.main;
        }

        public override void Activate(ITarget target, Transform attackSpot)
        {
            patternSpawnPoint = attackSpot;
            Activated = true;
            StartCoroutine(Charge());

        }

        protected void DealDamage(GameObject target)
        {
            StartCoroutine(DamageReload());
            target.TryGetComponent(out IHealth damageable);
            damageable?.TakeDamage(_damage);
        }

        private IEnumerator DamageReload()
        {
            _damageCooldown= true;
            yield return new WaitForSeconds(1f);
            _damageCooldown = false;
        }

        private IEnumerator Reload()
        {
            _OnReload = true;
            _followDistance = _followDistance + 3;

            yield return new WaitForSeconds(3f);
            _followDistance = _followDistance - 3;
            _OnReload = false;
        }

        private IEnumerator Charge()
        {

            transform.eulerAngles = new Vector3(90, 0, 0);
            var rend = _particle.GetComponent<ParticleSystemRenderer>();
            rend.material = _chargeMat;

            yield return new WaitForSeconds(_chargeDuration);
            rend.material = _targetMat;
            _isAttacking = true;
            yield return new WaitForSeconds(_attackDuration);
            _isAttacking = false;
            gameObject.SetActive(false);
            Activated = false;
        }

        void Update()
        {
            if (Activated)
            {
                transform.position = patternSpawnPoint.position;
                _storedPositions.Enqueue(_target.transform.position);
                if (_lastPos != null)
                {
                    var distance = Vector3.Distance(_target.transform.position, _lastPos);
                    if (distance >= 4 && !_OnReload)
                        StartCoroutine(Reload());
                }
                //if (_target.transform.position)
                if (_storedPositions.Count >= _followDistance)
                {
                    var pos = _storedPositions.Dequeue();

                    if (_isAttacking)
                    {
                        Vector3 direction = pos - transform.position;
                        Quaternion bulletRotation = Quaternion.LookRotation(direction, Vector3.up);
                        transform.rotation = bulletRotation;
                    }
                }
                _lastPos = _target.transform.position;



                var hits = Physics.SphereCastAll(transform.position, 2f, _target.transform.position, 400, ~transparentLayer, QueryTriggerInteraction.Ignore);
                Debug.DrawRay(transform.position, (_target.transform.position - transform.position) * 100, Color.yellow);
                if (hits.Length != 0)
                {
                    var closestHit = hits[0];

                    for (int i = 1; i < hits.Length; i++)
                    {
                        if (hits[i].distance < closestHit.distance)
                        {
                            closestHit = hits[i];
                        }
                    }
                    if (closestHit.collider.gameObject.layer == 7 )
                        DealDamage(GetComponent<Collider>().gameObject);
                }
            }
        }
    }
}