using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemysAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float stopDistance;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();  
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = enemySpeed;
        agent.stoppingDistance = stopDistance;
    }
    private void Update()
    {
        agent.SetDestination(player.position);
    }
}
