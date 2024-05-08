using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPress : MonoBehaviour
{
    [SerializeField] private GameObject[] _environmentObjects;
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("here");
        if (other.CompareTag("bullet") || other.CompareTag("Player"))
        {
            foreach (GameObject obj in listt)
            {
                obj.GetComponent<DeathByPlatform>().Stopp();
                Debug.Log("here");
            }

        }
    }*/
    void Start()
    {
        _environmentObjects = GameObject.FindGameObjectsWithTag("stoppableEnv");
    }

    void OnDisable()
    {
        foreach (GameObject obj in _environmentObjects)
        {
            obj.GetComponent<DeathByPlatform>().Stopp();
            Debug.Log("presses stop");
        }
    }
}
