using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitMarkEnemy : MonoBehaviour
{
    [SerializeField] private GameObject standardCross;
    [SerializeField] private GameObject redCross;

    // Start is called before the first frame update
    void Start()
    {
        standardCross.gameObject.SetActive(true);
        redCross.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                redCross.gameObject.SetActive(true);
                standardCross.gameObject.SetActive(false);
            }
            else
            {
                redCross.gameObject.SetActive(false);
                standardCross.gameObject.SetActive(true);
            }
        }

        
    }
}