using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWall : MonoBehaviour
{
    public bool Activated;
    [SerializeField]
    private float _damage;
    private float _projectileSpeed;
    private float _attackTime;
    private ShieldHealth _shield;
    private Vector3 _startPos;
    // Start is called before the first frame update
    protected void Awake()
    {
        _shield = GetComponentInChildren<ShieldHealth>();
        Physics.IgnoreLayerCollision(9, 9, true);
        Physics.IgnoreLayerCollision(6, 9, true);

    }
    void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    } 

    public void Activate(float damage, float projectileSpeed, float attackTime)
    {
        _damage = damage;
        
        _projectileSpeed = projectileSpeed;
        _attackTime = attackTime;
        _shield.gameObject.SetActive(true);
        
        StartCoroutine(Duration());
    }

    private IEnumerator Duration()
    {
        Activated = true;
        yield return new WaitForSeconds(_attackTime-0.01f);
        Activated = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Activated)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _projectileSpeed);
            
        }
    }
    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IHealth damageScript = collision.gameObject.GetComponent<IHealth>();
            if (damageScript != null)
            {
                Debug.Log("hit player");
                damageScript.TakeDamage(_damage);

                Activated = false;
                gameObject.SetActive(false);
            }
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
}
