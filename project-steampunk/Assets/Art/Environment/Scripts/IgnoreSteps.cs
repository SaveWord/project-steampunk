using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IgnoreSteps : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepSmooth = 2f;


    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        stepClimb();
    }

    void stepClimb()
    {
        Vector3 movedir= Vector3.zero;
        if (Keyboard.current[Key.W].isPressed)
            movedir += transform.forward;
        if (Keyboard.current[Key.S].isPressed)
            movedir -= transform.forward;
        if (Keyboard.current[Key.A].isPressed)
            movedir -= transform.right;
        if (Keyboard.current[Key.D].isPressed)
            movedir += transform.right;
        //Debug.Log($"Movedir={movedir}");
        //Debug.DrawRay(transform.position, movedir*10, Color.red, 1f);
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, movedir, out hitLower, 0.75f))
        {
            //Debug.Log("Obstacle");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, movedir, out hitUpper, 1f))
            {
                //Debug.Log("Steps0123");
                rigidBody.position -= new Vector3(0f, -stepSmooth*Time.deltaTime, 0f);
            }
        }
    }
}
