using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Magnet : Bullet
{
    [SerializeField] private float MagnetRad;
    [SerializeField] private float TurnAngle;

    [SerializeField] private float _lifettime;
    private float distance;
    private float _turntime;
    Vector3 targetDirection;
    Quaternion targetRotation;

    private void Start()
    {
        targetDirection = (targetObject.transform.position - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(targetDirection);
    }
    public override void OnFly()
    {
        _timeOnFly += Time.deltaTime;
        _turntime += Time.deltaTime;
        if (_timeOnFly >= _lifettime) Destroy(gameObject);


        distance = Vector3.Distance(gameObject.transform.position, targetObject.transform.position);
        if (distance < MagnetRad)
        {
            transform.rotation = targetRotation;
            _rBody.velocity = transform.forward * _speed;
        }
        else
        {
            targetDirection = (targetObject.transform.position - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = targetRotation;
            _rBody.velocity = transform.forward * _speed;
        }
       
    }
}
