using Enemies.BossMoves;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class BossMover : MonoBehaviour
    {
        [Header("Basics")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Dash _dash;

        private NavMeshAgent _nMeshAgent;
        private Rigidbody _rBody;
        private controlarrow _controlarrow;
        private Animator _animator;

        public void MoveToTarget(ITarget target) //переиспользование на уровне, конечно
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

        public void MoveTo(Vector3 position)
        {
            if (_nMeshAgent.enabled)
            {
                _nMeshAgent.SetDestination(position);

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

        public void Dash()
        {
            if (_dash.IsDashCharged)
                _dash.MakeDash(ToggleControlToRBody);
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

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.transform.gameObject.layer == 8 && !_nMeshAgent.enabled)
                ToggleControlToNavMesh();
        }

        private void Awake()
        {
            _nMeshAgent = GetComponent<NavMeshAgent>();
            _rBody = GetComponent<Rigidbody>();
            
            _dash = new Dash(_rBody, this);
            _controlarrow = GetComponent<controlarrow>();

            _animator = GetComponentInChildren<Animator>();
        }

    }
}

