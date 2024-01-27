using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blowUp : MonoBehaviour
{
    public bool PicketUp = true;
    [SerializeField] GameObject area;
    private bool blown = false;
    void Start()
    {
        
    }
    public void Blow()
    {
        PicketUp = true;
        Debug.Log("blow");
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!blown)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;
                GameObject spawnedObject = Instantiate(area, spawnPosition, spawnRotation);
                PicketUp = false;
                Debug.Log("Destroy");
                Destroy(gameObject);
                blown = true;
            }

           
            //if (PicketUp)
            //{
            //    Vector3 spawnPosition = transform.position;
            //    Quaternion spawnRotation = transform.rotation;
            //    //GameObject spawnedObject = Instantiate(area, spawnPosition, spawnRotation);
            //    GameObject spawnedObject = Instantiate(area, spawnPosition, spawnRotation);
            //    PicketUp = false;
            //    //spawnedObject.transform.parent = transform;
            //    //PicketUp = false;
            //    Debug.Log("Destroy");
            //    Destroy(gameObject);
            //}
        }
    }
}
