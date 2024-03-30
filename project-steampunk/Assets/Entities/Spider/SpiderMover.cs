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
        [SerializeField] private float _moveSpeed = 5f;
        //[SerializeField] private Dash _dash;

        private NavMeshAgent _nMeshAgent;
        ///[SerializeField] 
        private Rigidbody _rBody;
        private controlarrow _controlarrow;
        private Animator _animator;


        public float dashForce = 100f;
        public float dashDuration = 2f;
        public float dashCooldown = 1f;
        private bool canDash = true;

        private float distance;
        private Vector3 targetPosition;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float DashDistance;
        [SerializeField] private float _damageCloseCombat;
        [SerializeField] private float _damageJump;
        [SerializeField] private float dashoffset;
        [SerializeField] private float Cooldowntime;
        [SerializeField] private float JumpCooldowntime;

        private bool damagecooldown = false;
        private bool jumpdamagecooldown = false;

        public void MoveToTarget(ITarget target)
        {

            if (_nMeshAgent.enabled)
            {
                _nMeshAgent.SetDestination(target.GetPosition());



                _animator.SetBool("isRunning", true);

                //_controlarrow.ChangeColorToGray();
                //_controlarrow.Show();
            }
            else
            {
                _animator.SetBool("isRunning", false);
                //_controlarrow.Hide();
            }


        }

        public void Dash(ITarget target)
        {
            distance = Vector3.Distance(transform.position, target.GetPosition());//
            targetPosition = target.GetPosition();

            if (!canDash)
            {
                return;
            }
            if (jumpdamagecooldown)
            {
                return;
            }
            if (distance < (DashDistance + dashoffset) && distance > (DashDistance - dashoffset))
            {
                StartCoroutine(GoDash(dashDuration));
                StartCoroutine(JumpCooldown(JumpCooldowntime));
            }

        }
        IEnumerator GoDash(float time)
        {
            canDash = false;
            ToggleControlToRBody();
            Vector3 dashDirection = (targetPosition - transform.position).normalized;


            _rBody.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
            _rBody.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            yield return new WaitForSeconds(time);
            ToggleControlToNavMesh();
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;

        }
        private void ToggleControlToRBody()
        {
            _rBody.isKinematic = false;
            _rBody.useGravity = true;
            _nMeshAgent.enabled = false;
        }

        private void ToggleControlToNavMesh()
        {
            _rBody.isKinematic = true;
            _rBody.useGravity = false;
            _nMeshAgent.enabled = true;
        }

        //private void OnCollisionStay(Collision collision)
        //{
        //    if (collision.collider.transform.gameObject.layer == 8 && !_nMeshAgent.enabled)
        //        ToggleControlToNavMesh();
        //}

        private void Awake()
        {
            _nMeshAgent = GetComponent<NavMeshAgent>();
            _rBody = GetComponent<Rigidbody>();

            //_dash = new Dash(_rBody, this);
            //_controlarrow = GetComponent<controlarrow>();

            _animator = GetComponentInChildren<Animator>();
            _moveSpeed = GameObject.FindWithTag("Player").GetComponent<PlayerMove>().GetSpeed() * 0.9f;

        }

        private void OnDrawGizmos()
        {
            if (canDash)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.position + Vector3.up * 10, Vector3.one * 3f);
            }
            else
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(transform.position + Vector3.up * 10, Vector3.one * 3f);
            }

        }

        protected void OnCollisionEnter(Collision collision)
        {
            
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("hit player");
                HpHandler damageScript = collision.gameObject.GetComponent<HpHandler>();
                if (canDash && !damagecooldown)
                {
                    damageScript.TakeDamage(_damageCloseCombat);
                    StartCoroutine(Cooldown(Cooldowntime));
                }
                else if (!damagecooldown) { 
                    damageScript.TakeDamage(_damageJump);
                    StartCoroutine(Cooldown(Cooldowntime));
                }
                
            }
        }
        IEnumerator Cooldown(float time)
        {
            damagecooldown = true;

            yield return new WaitForSeconds(time);
            damagecooldown = false;

        }
        IEnumerator JumpCooldown(float time)
        {
            jumpdamagecooldown = true;

            yield return new WaitForSeconds(time);
            jumpdamagecooldown = false;

        }
    }
}

