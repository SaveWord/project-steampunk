using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LegStepper : MonoBehaviour
{
    [SerializeField] private Transform homeTransform;
    [SerializeField] private float stepDistance;
    [SerializeField] private float stepTime;
    [SerializeField] private float stepOvershootFraction;

   Vector3 currentPosition;

    public bool moving;

    private void Awake()
    {
        transform.SetParent(null);
    }
    public void TryToMove()
    {
        if (moving) return;
        if(Vector3.Distance(transform.position, homeTransform.position) > stepDistance)
        {
            StartCoroutine(StepLeg());
        }
    }
    IEnumerator StepLeg()
    {
        moving = true;

        Quaternion startRot = transform.rotation;
        Vector3 startPoint = transform.position;
        Quaternion endRot = homeTransform.rotation;

      
        Vector3 towardHome = (homeTransform.position - transform.position);
        float overShootDistance = stepDistance * stepOvershootFraction;

        Vector3 overShootVector = towardHome * overShootDistance;
        overShootVector = Vector3.ProjectOnPlane(overShootVector, Vector3.up);
        Vector3 endPoint = homeTransform.position + overShootVector;
        Vector3 centerPoint = (startPoint + endPoint) / 2;


        float timeElapsed = 0;



        do
        {
        
            timeElapsed += Time.deltaTime;

            float normalizedTime = timeElapsed / stepTime;
            normalizedTime = Easing.InCubic(normalizedTime);


            transform.position = Vector3.Lerp(
        Vector3.Lerp(startPoint, centerPoint, normalizedTime),
        Vector3.Lerp(centerPoint, endPoint, normalizedTime),
        normalizedTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }
        while (timeElapsed < stepTime);   
        moving = false;
    }
}
