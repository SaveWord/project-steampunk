using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] float X_speed;
    [SerializeField] float Y_speed;
    [SerializeField] float Z_speed;
    private void FixedUpdate()
    {
        transform.Rotate(X_speed * Time.deltaTime, Y_speed * Time.deltaTime, Z_speed * Time.deltaTime);
    }
}
