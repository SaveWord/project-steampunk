using Enemies.AntMoves;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class AntMover : MonoBehaviour
    {
        [Header("Basics")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Dash _dash;

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
        [SerializeField] private bool ReverceDash;
        [SerializeField] private float DashForwardReactDist;
        [SerializeField] private float DashBackwardReactDist;
        [SerializeField] private bool JumpUp;


        public void MoveToTarget(ITarget target)
        {
            if (_nMeshAgent.enabled)
            {
                _nMeshAgent.SetDestination(target.GetPosition());

                

                _animator.SetBool("isRunning", true);

                _controlarrow.ChangeColorToGray();
                _controlarrow.Show();
            }
            else
            {
                _animator.SetBool("isRunning", false);
                _controlarrow.Hide();
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

            bool shouldDash = (!ReverceDash && distance > DashForwardReactDist) || (ReverceDash && distance < DashBackwardReactDist);
            if (shouldDash)
            {
                StartCoroutine(GoDash(dashDuration));
            }

        }
        IEnumerator GoDash(float time)
        {
            canDash = false;
            ToggleControlToRBody();
            Vector3 dashDirection = (targetPosition - transform.position).normalized;
            if(ReverceDash) { dashDirection = -dashDirection; }

            if (JumpUp)
            {
                _rBody.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
                _rBody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            }
            else
            {
                _rBody.AddForce(Vector3.up * dashForce* 0.2f, ForceMode.Impulse);
                _rBody.AddForce(dashDirection * dashForce*4f, ForceMode.Impulse);
            }
            
           

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
            
            _dash = new Dash(_rBody, this);
            _controlarrow = GetComponent<controlarrow>();

            _animator = GetComponentInChildren<Animator>();
            
        }

        private void OnDrawGizmos()
        {
            if(canDash)
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

    }
}

