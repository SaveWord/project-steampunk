using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blowUp : MonoBehaviour
{
    public bool PicketUp = false;
    [SerializeField] GameObject area;
    void Start()
    {
        
    }
    public void Blow()
    {
        PicketUp = true;
        Debug.Log("blow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (PicketUp)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;
                //GameObject spawnedObject = Instantiate(area, spawnPosition, spawnRotation);
                GameObject spawnedObject = Instantiate(area, spawnPosition, spawnRotation);
                //spawnedObject.transform.parent = transform;
                //PicketUp = false;
                Destroy(gameObject);
            }
        }
    }
}
