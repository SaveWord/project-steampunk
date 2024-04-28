using Enemies.SpiderStates;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class SpiderMover : MonoBehaviour
    {
        [Header("Basics")]
        private NavMeshAgent _nMeshAgent;
        //private Rigidbody _rBody;
        private Animator _animator;
        private float distance;
        private float speed;
        [SerializeField] private NavMeshAgent navMeshAgent;

        [Tooltip("jumps if the player is closer then this"),SerializeField] private float DistanceToJump; //jumps if the player is closer then this
        [Tooltip("speed is multiplied by this factor when jumping"),SerializeField] private float speedFactor;  //speed is multiplied by this factor when jumping 
        [Tooltip("speed returns to normal after this amount of time"), SerializeField] private float jumpTime;  //speed returns to normal after this amount of time

        [Tooltip("deals this amount of damage when colliding"), SerializeField] private float _damageCloseCombat;  //deals this amount of damage when colliding 
        [Tooltip("deals this amount of damage when jumping"), SerializeField] private float _damageJump;  //deals this amount of damage when jumping
        [Tooltip("Cooldown damage")]
        [SerializeField] private float DamageCooldown; //pause between dealing damage

        private float time = 0f;
        private float damagetime = 0f;
        private bool jumping;
        private bool hurt;

        public void MoveToTarget(ITarget target)
        {
            if (_nMeshAgent.enabled)
            {
                _nMeshAgent.SetDestination(target.GetPosition());
                _animator.SetBool("isRunning", true);

                distance = Vector3.Distance(transform.position, target.GetPosition());
                if (distance<DistanceToJump&&!jumping)
                {
                    SpeedForward();
                }
            }
            else
            {
                _animator.SetBool("isRunning", false);
            }
        }

        public void SpeedForward() //new speed for jump
        {
            navMeshAgent.speed *= speedFactor;
            jumping = true;
        }
        public void ResetSpeed() //return old speed
        {
            navMeshAgent.speed = speed;
            jumping = false;
            time = 0;
        }

        //public void Dash(ITarget target)
        //{
        //    distance = Vector3.Distance(transform.position, target.GetPosition());//
        //    targetPosition = target.GetPosition();

        //    if (!canDash)
        //    {
        //        return;
        //    }
        //    if (jumpdamagecooldown)
        //    {
        //        return;
        //    }
        //    if (distance < (DashDistance + dashoffset) && distance > (DashDistance - dashoffset))
        //    {
        //        StartCoroutine(GoDash(dashDuration));
        //        StartCoroutine(JumpCooldown(UnityEngine.Random.Range(1,5)));
        //    }

        //}
        //IEnumerator GoDash(float time)
        //{
        //    canDash = false;
        //    ToggleControlToRBody();
        //    Vector3 dashDirection = (targetPosition - transform.position).normalized;


        //    _rBody.AddForce(Vector3.up * dashForceUp, ForceMode.Impulse);
        //    _rBody.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        //    yield return new WaitForSeconds(time);
        //    ToggleControlToNavMesh();
        //    yield return new WaitForSeconds(dashCooldown);
        //    canDash = true;

        //}
        private void ToggleControlToRBody()
        {
            //_rBody.isKinematic = false;
            //_rBody.useGravity = true;
            _nMeshAgent.enabled = false;
        }

        private void ToggleControlToNavMesh()
        {
            //_rBody.isKinematic = true;
            //_rBody.useGravity = false;
            _nMeshAgent.enabled = true;
        }

        private void Awake()
        {
            _nMeshAgent = GetComponent<NavMeshAgent>();
            //_rBody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            speed = navMeshAgent.speed;

        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("hit player");
                IHealth damageScript = collision.gameObject.GetComponent<IHealth>();
                damagetime = Time.time + DamageCooldown;
                if (damagetime > Time.time)
                {
                    if (!jumping)
                    {
                        damageScript.TakeDamage(_damageCloseCombat);
                        hurt = true;
                    }
                    else
                    {
                        damageScript.TakeDamage(_damageJump);
                        hurt = true;
                    }
                }
                
            }
        }
        
        public void Update()
        {
            if (jumping) time += Time.deltaTime; //timer for jump
            if (time >= jumpTime) ResetSpeed();

            if(hurt) damagetime += Time.deltaTime; //timer for damage
            //if (damagetime >= DamageCooldown)
            //{
            //    hurt = false;
            //    damagetime = 0;
            //}
        }
    }
}

