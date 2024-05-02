using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPress : MonoBehaviour
{
    [SerializeField] private List<GameObject> listt = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
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
    }
}
