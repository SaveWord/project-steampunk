using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserFollow : MonoBehaviour
{
    [Header("Laser Parametres")]
    
    private ITarget playerTarget;

    public GameObject _pointOfAttack;
    public float _followDistance;

    public float _damage = 1;
    public float _speed;
    public float _attackDuration;

    [Header("Visual Parametres")]
    private List<Vector3> _storedPositions;
    private LineRenderer _lineRenderer;
    private MeshCollider _meshCollider;

    
    private float startFollowingTime;

    public void LaserFollowInstanciate(GameObject pointOfAttack, float followDistance, float damage, float speed, float attackDuration)
    {
        _pointOfAttack = pointOfAttack;
        _followDistance = followDistance;

        _damage = damage;
        _speed = speed;
        _attackDuration = attackDuration;
    }
    void Awake()
    {
        gameObject.SetActive(false);
        _lineRenderer = GetComponent<LineRenderer>();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        _storedPositions = new List<Vector3>(); 
        /*player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.Log("The FollowingMe gameobject could not be found");
        }
*/
        if (_followDistance == 0)
        {
            Debug.Log("Please set distance higher then 0");
        }
    }

    public void AttackCycle(ITarget target)
    {
        if (target==null)
            Debug.Log("The FollowingMe gameobject could not be found");
        else
            playerTarget = target;
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerTarget.GetPosition().x - 6, 2, playerTarget.GetPosition().z - 6), 2);
        StartCoroutine(SetDuration()); 
    }
    private IEnumerator SetDuration()
    {
        yield return new WaitForSeconds(_attackDuration);
        gameObject.SetActive(false);
    }
    private IEnumerator Attack(Vector3 position, float offset, float starttime)
    {
        yield return new WaitForSeconds(offset);

        float distCovered = (Time.time - starttime) * _speed;
        var journeyLength = Vector3.Distance(transform.position, new Vector3(position.x, 2, position.z));
        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, 2, position.z), fractionOfJourney);
    }

    void Update()
    {
        //move
        startFollowingTime = Time.time;
        StartCoroutine(Attack(playerTarget.GetPosition(), _followDistance, startFollowingTime));
        //draw line laser
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _pointOfAttack.transform.position+new Vector3(0,2,0));
        _lineRenderer.SetPosition(1, transform.position);
    }

    protected void DealDamage(GameObject target)
    {
        target.TryGetComponent(out IHealth damageable);
        damageable?.TakeDamage(_damage);
        Debug.Log("attack from ring");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }
}
