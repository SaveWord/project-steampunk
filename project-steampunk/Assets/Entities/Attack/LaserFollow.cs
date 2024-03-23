using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserFollow : MonoBehaviour
{
    public GameObject player;
    public float followDistance;
    private List<Vector3> storedPositions;
    private LineRenderer _lineRenderer;
    private MeshCollider _meshCollider;
    public float _damage = 1;
    public GameObject _pointOfAttack;
    private float startTime;
    public float speed;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _meshCollider = gameObject.AddComponent<MeshCollider>();

        storedPositions = new List<Vector3>(); 

        if (!player)
        {
            Debug.Log("The FollowingMe gameobject was not set");
        }

        if (followDistance == 0)
        {
            Debug.Log("Please set distance higher then 0");
        }
    }

    private IEnumerator DestructTime(Vector3 position, float offset, float starttime)
    {

        yield return new WaitForSeconds(offset);

        float distCovered = (Time.time - starttime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        var journeyLength = Vector3.Distance(transform.position, new Vector3(position.x, 2, position.z));
        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, 2, position.z), fractionOfJourney);

        //transform.position = new Vector3( position.x, 2, position.z);
    }

    void Update()
    {
        startTime = Time.time;

        // Calculate the journey length.
        StartCoroutine(DestructTime(player.transform.position, followDistance, startTime));

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
        Debug.Log("shosh " + collision.gameObject.GetInstanceID());

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DealDamage(collision.gameObject);

        }

    }
}
