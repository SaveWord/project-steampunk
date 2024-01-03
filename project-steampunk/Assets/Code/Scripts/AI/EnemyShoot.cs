using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform positionRespawnBullet;
    [SerializeField] private float missEnemy;
    [SerializeField] private float bulletSpeed;
    private Transform playerTarget;
    private float interval;
    private float timer;
    private Vector3 playerLastPosition;
    Vector3 targetPosition;
    private void Start()
    {
       
      playerTarget = GameObject.FindWithTag("Player").transform;
        playerLastPosition = playerTarget.position;
    }
    private void Update()
    {
        //targetPosition = playerTarget.position + (Trajectory() * TimeWay());
            targetPosition = playerTarget.position;
            if (timer > interval && Vector3.Distance(transform.position, playerTarget.position) <= 10)
            {
                ResetTimer();
                ShootBullet();
            }
            timer += Time.deltaTime;
            transform.LookAt(playerTarget);
    }


    private void ResetTimer()
    {
        timer -= interval;
        interval = Random.Range(1f, 4f);
    }
    private void ShootBullet()
    {
        bullet.GetComponent<bulletMove>().speed = bulletSpeed;
        GameObject newBullet = Instantiate(bullet, positionRespawnBullet);
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), GetComponent<Collider>());
        newBullet.transform.localPosition = new Vector3(0, 0, 0); 
        newBullet.transform.LookAt(targetPosition);
        //newBullet.transform.LookAt(targetPosition + AimMiss(missEnemy));
        newBullet.GetComponent<bulletMove>().BulletMove();

    }
    //private float TimeWay()
    //{
    //    float distance = Vector3.Distance(transform.position, playerTarget.position);
    //    return distance;
    //}
    //private Vector3 Trajectory()
    //{
    //    Vector3 trajectory = (playerTarget.position - playerLastPosition) / Time.deltaTime;
    //    playerLastPosition = playerTarget.position;
    //    return trajectory;
    //}
    //private Vector3 AimMiss(float value)
    //{
    //    return Random.insideUnitSphere * value;
    //}
}
