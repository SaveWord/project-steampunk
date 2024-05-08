using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Enemies.Bullets
{
    public class Ring : Bullet
    {
        public ITarget Target;

        [Header("Basics")]
        [SerializeField] private float _damage;
        [SerializeField] private float _waveSpeed;
        //[SerializeField] private bool FollowForSomeTime;

        private GameObject targetObject;
        //private float followDuration = 15f;
        // private Vector3 lastKnownPosition;

        private Rigidbody _rBody;
        private float _timeOnFly;


        private LineRenderer _lineRenderer;
        private MeshCollider _meshCollider;
        [SerializeField] private float _startingRadius = 5f;
        [SerializeField] private int subdivision = 10;
        private bool _OnReload =false ;
        [SerializeField] private float _lifetime = 10f;
        private bool a = true;
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _meshCollider = gameObject.AddComponent<MeshCollider>();
            //_rBody = GetComponent<Rigidbody>();
            // targetObject = GameObject.FindGameObjectWithTag("Player");
            //transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        private void Start()
        {
            AudioManager.InstanceAudio.PlaySfxEnemy("EnemyAttackRing");
            transform.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(DestructTime(_lifetime));
        }

        private void Update()
        {
            _startingRadius += Time.deltaTime * _waveSpeed;
             float angleStep = 2f * Mathf.PI / subdivision;
            _lineRenderer.positionCount = subdivision;
            for (int i = 0; i < subdivision; i++)
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
        private void OnFly()
        {
            _timeOnFly += Time.deltaTime;
            gameObject.transform.localScale += new Vector3(1 * _waveSpeed, 0, 1 * _waveSpeed);
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
            Destroy(gameObject);
        }
        private IEnumerator DamageTime(float reloadTime)
        {

            yield return new WaitForSeconds(reloadTime);
            _OnReload = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("shosh " + collision.gameObject.GetInstanceID());
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Props"))
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_OnReload)
            {
                _OnReload = true;
                DealDamage(collision.gameObject);
                StartCoroutine(DamageTime(3f));

            }

        }

       

    }
}

