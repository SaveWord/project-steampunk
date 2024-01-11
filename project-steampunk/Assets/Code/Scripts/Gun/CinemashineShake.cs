using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemashineShake : MonoBehaviour
{
    CinemachineImpulseSource impulse;
    // Start is called before the first frame update
    void Start()
    {
        impulse = transform.GetComponent<CinemachineImpulseSource>();
        //Invoke("Shake",3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Invoke("Shake", 0f);
    
    }

    void Shake()
    {
        impulse.GenerateImpulse();
    }
}
