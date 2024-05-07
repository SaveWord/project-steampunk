using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Enemies.Bullets
{
    public class Ring : Bullet
    {
        [Header("Ring Basics")]

        [SerializeField] 
        private float _waveSpeed;
        [SerializeField] 
        private float _startingRadius = 5f;
        [SerializeField] 
        private int _subdivision = 30;

        private MeshCollider _meshCollider;
        private LineRenderer _lineRenderer;
        private bool _isOnReload =false ;
        // [SerializeField] private float _lifeTime = 10f;
        private bool _isAttackReset = false;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _meshCollider = gameObject.AddComponent<MeshCollider>();
            //transform.rotation = Quaternion.Euler(0, 0, 0);

        }

        private void OnEnable()
        {
            //TODO: restore and add prefab to ant
            // var audioSource = transform.parent.gameObject.GetComponentInParent<AudioSource>();
            //AudioManager.InstanceAudio.PlaySfxEnemy("EnemyAttackRing");
            _isAttackReset = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {
            if (_isAttackReset)
                _startingRadius = 5f;
            else
            {
                _startingRadius += Time.deltaTime * _waveSpeed;
                float angleStep = 2f * Mathf.PI / _subdivision;
                _lineRenderer.positionCount = _subdivision;
                for (int i = 0; i < _subdivision; i++)
                {
                    float xPosition = _startingRadius * Mathf.Cos(angleStep * i);
                    float zPosition = _startingRadius * Mathf.Sin(angleStep * i);
                    Vector3 pointInCircle = new Vector3(xPosition, 0f, zPosition);
                    _lineRenderer.SetPosition(i, pointInCircle);

                }
                Mesh mesh = new Mesh();
                _lineRenderer.BakeMesh(mesh, true);
                _meshCollider.sharedMesh = mesh;
                //_meshCollider.isTrigger = true;
                _meshCollider.enabled = true;
            }
        }

        private void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            gameObject.transform.localScale += new Vector3(1 * _waveSpeed, 0, 1 * _waveSpeed);
        }

        override public void StartFly(Vector3 direction)
        {
            _startingRadius = 5f;
            // _collider = gameObject.GetComponent<SphereCollider>();
            // _collider.enabled = true;
            //transform.LookAt(direction);
            StartCoroutine(DestructTime(_lifeTime));
            //_rBody.velocity = transform.forward * _speed;
        }

        protected void DealDamage(GameObject target)
        {
            target.TryGetComponent(out IHealth damageable);
            damageable?.TakeDamage(_damage);
            Debug.Log("attack from ring"+_damage);
        }

        private IEnumerator DestructTime(float reloadTime)
        {
            
            yield return new WaitForSeconds(reloadTime);
            _startingRadius = 5f;
            _isAttackReset = true;
            gameObject.SetActive(false);
        }

        private IEnumerator DamageTime(float reloadTime)
        {

            yield return new WaitForSeconds(reloadTime);
            _isOnReload = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Props"))
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_isOnReload)
            {
                _isOnReload = true;
                DealDamage(collision.gameObject);
                StartCoroutine(DamageTime(3f));

            }
        }
    }
}

