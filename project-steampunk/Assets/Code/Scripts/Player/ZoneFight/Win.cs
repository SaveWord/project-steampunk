using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public GameObject[] myObjects;
    private void OnTriggerStay(Collider other)
    {
        bool allNull = true;
        foreach (GameObject obj in myObjects)
        {
            if (obj != null)
            {
                allNull = false;
                break;
            }
        }
        if (allNull)
        {
            Destroy(gameObject);
        }
    }

}
