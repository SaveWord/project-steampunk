using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] float speed;
    private void Update()
    {
        transform.Rotate(-speed * Time.deltaTime, 0, 0);
    }
}
