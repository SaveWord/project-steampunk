using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneFight : MonoBehaviour
{
    public GameObject[] myObjects;

    void Update()
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
