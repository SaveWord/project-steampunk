using Enemies.AntMoves;
using System;
using System.Collections;
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
        private Rigidbody _rBody;

        public void MoveToTarget(ITarget target)
        {
            if(_nMeshAgent.enabled)
                _nMeshAgent.SetDestination(target.GetPosition());
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
            if (collision.collider.tag == "Ground" && !_nMeshAgent.enabled)
                ToggleControlToNavMesh();
        }

        private void Awake()
        {
            _nMeshAgent = GetComponent<NavMeshAgent>();
            _rBody = GetComponent<Rigidbody>();
            _dash = new Dash(_rBody, this);
        }

    }
}

