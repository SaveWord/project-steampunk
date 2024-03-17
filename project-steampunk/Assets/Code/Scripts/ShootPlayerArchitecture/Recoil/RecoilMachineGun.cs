using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilMachineGun : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    [SerializeField] private float angleClamp;

    //HipFire
    [SerializeField]private float recoilX;

    //slerp smooth time
    [SerializeField] private float smoothTime;
    [SerializeField] private float returnSpeed;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, smoothTime * Time.fixedDeltaTime);
        currentRotation.x = Mathf.Clamp(currentRotation.x, -angleClamp, angleClamp);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void Recoil()
    {
        targetRotation += new Vector3(recoilX, 0, 0);
    }
}
