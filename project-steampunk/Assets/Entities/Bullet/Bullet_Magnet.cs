using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Magnet : Bullet
{
    [SerializeField] private float MagnetRad;
    [SerializeField] private float TurnAngle;
    private float distance;
    private float rotationTime;
    private bool go = false;
    private Vector3 continueDirection;

    public override void OnFly()
    {
        _timeOnFly += Time.deltaTime;
        if (_timeOnFly >= _lifeTime) SelfDestroy();
        if (targetObject != null)
        {
            distance = Vector3.Distance(gameObject.transform.position, targetObject.transform.position);
            
            if (distance < MagnetRad)
            {
                go = false;
                Debug.Log("affcet");
                if((targetObject.transform.position - transform.position).normalized != (lastKnownPosition - transform.position).normalized)
                {
                    float angle = Vector3.Angle((targetObject.transform.position - transform.position).normalized, (lastKnownPosition - transform.position).normalized);
                    Debug.Log("Angle between vectors: " + angle);
                    if(angle > 10f) {

                    
                    }
                }
                rotationTime += Time.deltaTime;
                Vector3 direction = targetObject.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnAngle * rotationTime);
            }
            else
            {
                if (!go)
                {
                    rotationTime = 0f;
                    continueDirection = (lastKnownPosition - transform.position).normalized;
                    go = true;
                }
                Debug.Log(continueDirection);
                transform.position += continueDirection * _speed * Time.deltaTime;
                Debug.Log(transform.position);
            }
        }
        else
        {
            Debug.LogWarning("Target object not found!");
        }
    }
}
