using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{


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
    public GameObject _pointOfAttack;
    public GameObject _target;

    public int _followDistance;
    public float _damage = 2;
    public float _attackDuration;
    public float _chargeDuration;
    private bool _isAttacking = false;

    [Header("Visual Parametres")]
    public Material _chargeMat;
    public Material _targetMat;
    private Queue<Vector3> _storedPositions = new Queue<Vector3>();
    private ParticleSystem particle;

    void Start()
    {
       particle = GetComponent<ParticleSystem>();
        gameObject.SetActive(false);
    }

    protected void DealDamage(GameObject target)
    {
        target.TryGetComponent(out IHealth damageable);
        damageable?.TakeDamage(_damage);
        Debug.Log("attack from ll ");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }
    public void StartAttack() {

        StartCoroutine(Charge());

    }
    private IEnumerator Charge()
    {

        transform.eulerAngles = new Vector3(90,0,0);
        var rend = particle.GetComponent<ParticleSystemRenderer>();
        rend.material = _chargeMat;

        yield return new WaitForSeconds(_chargeDuration);
        rend.material = _targetMat;
        _isAttacking = true;
        yield return new WaitForSeconds(_attackDuration);
        _isAttacking = false;
        gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = _pointOfAttack.transform.position;
        _storedPositions.Enqueue(_target.transform.position);

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

    }
}
