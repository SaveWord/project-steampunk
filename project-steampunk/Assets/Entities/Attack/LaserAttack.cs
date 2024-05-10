using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Attacks.Attacks
{
    public class LaserAttack : AttackBaseClass
    {
        public bool Activated { get; set; }

        [Header("Attack Parametres")]
        public ITarget _target;

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
        [SerializeField]
        private bool _damageCooldown = false;

        [SerializeField] private EnemyAudioCollection _audioSource;

        void Awake()
        {
            _audioSource = transform.parent.gameObject.transform.parent.GetComponentInChildren<EnemyAudioCollection>();
            _particle = GetComponent<ParticleSystem>();
            gameObject.SetActive(false);
        }

        public override void Activate(ITarget target, Transform attackSpot)
        {
            _target = target;
            patternSpawnPoint = attackSpot;
            Activated = true;
            _damageCooldown = false;
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
            yield return new WaitForSeconds(0.4f);
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
            Activated = false;

            _audioSource.sfxSource.loop = false;
            _audioSource.sfxSource.Stop();
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (Activated)
            {
                _audioSource.sfxSource.loop = true;
                _audioSource.PlaySfxEnemy("EnemyAttackLaser");
                transform.position = patternSpawnPoint.position;
                _storedPositions.Enqueue(_target.GetPosition());
                if (_lastPos != null)
                {
                    var distance = Vector3.Distance(_target.GetPosition(), _lastPos);
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

                        var hits = Physics.SphereCastAll(transform.position, 1f, (pos - transform.position), 400, ~transparentLayer, QueryTriggerInteraction.Ignore);
                        Debug.DrawRay(transform.position, (pos - transform.position), Color.yellow);
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
                            Debug.Log("suqa " + closestHit.collider.gameObject.layer);
                            if (closestHit.collider.gameObject.layer == 7 && !_damageCooldown)
                                DealDamage(closestHit.collider.gameObject);
                        }

                    }
                }
                _lastPos = _target.GetPosition();



               
            }
        }
    }
}