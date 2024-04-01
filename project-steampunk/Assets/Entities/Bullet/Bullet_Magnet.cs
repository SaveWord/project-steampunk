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

    public override void OnFly()
    {
        _timeOnFly += Time.deltaTime;
        _turntime += Time.deltaTime;
        if (_timeOnFly >= _lifettime) SelfDestroy();
        if (targetObject != null)
        {
            distance = Vector3.Distance(gameObject.transform.position, targetObject.transform.position);
            if (distance < MagnetRad && _turntime > 0.5f)
            {
                Vector3 targetDirection = (targetObject.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
                if (angleDifference <= TurnAngle)
                {
                    transform.rotation = targetRotation;
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnAngle);
                }
                _rBody.velocity = transform.forward * _speed;
                _turntime = 0;
            }
        }
        else
        {
            Debug.LogWarning("Target object not found!");
        }
        void SelfDestroy()
        {
            Debug.Log("Die");
            StartCoroutine(SelfDestroyCoroutine());
        }

        IEnumerator SelfDestroyCoroutine()
        {
            sphereDie.SetActive(true);
            yield return new WaitForSeconds(coroutineTimeDie);
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider collision)
        {

            IHealth damageScript = collision.gameObject.GetComponent<IHealth>();
            if (damageScript != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("hit player");
                damageScript.TakeDamage(_damage);
                SelfDestroy();
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Props"))
            {
                SelfDestroy();
            }

        }
    }
}
