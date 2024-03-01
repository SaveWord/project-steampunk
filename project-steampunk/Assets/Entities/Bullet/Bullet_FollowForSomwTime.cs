using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_FollowForSomwTime : Bullet
{
    [SerializeField] private float followDuration = 15f;
    protected float _turntime;
    public override void OnFly()
    {
        _timeOnFly += Time.deltaTime;
        _turntime += Time.deltaTime;
        if (_timeOnFly >= _lifeTime) SelfDestroy();
        if (targetObject != null && _turntime > 0.5f && _timeOnFly < followDuration)
        {
            //Debug.Log("follow");
            Vector3 targetDirection = (targetObject.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = targetRotation;
            _rBody.velocity = transform.forward * _speed;
            _turntime = 0;
        }
    }
}
