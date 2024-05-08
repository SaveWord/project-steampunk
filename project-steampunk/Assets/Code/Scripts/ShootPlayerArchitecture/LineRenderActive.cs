using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderActive : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private float timeActive;
    private float timeAll = 0;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (timeAll == 0 && line.enabled == true) timeAll = Time.time + timeActive;
        if(Time.time > timeAll && line.enabled == true)
        {
            line.enabled = false;
            timeAll = 0;
        }
        
    }
}
