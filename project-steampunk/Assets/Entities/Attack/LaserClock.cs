using Enemies.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Attacks.Attacks
{
    public class LaserClock : MonoBehaviour
    {
        [Tooltip(" The number of tics to create")]
        public int numTics = 12;
        [Tooltip("Diameter of the clock rose")]
        public float roseDiameter = 5f;
        [Tooltip("The length of each tic")]
        public float ticLength = 0.5f;
        [Tooltip("The width of each tic")]
        public float ticWidth = 0.05f;
        [Tooltip("Rotation speed in angle per second")]
        public float anglePerSecond = 0.1f;
        [Tooltip("Material to use for the tics")]
        public Material ticMaterial;
        [Tooltip("damage")]
        public float _damage;

        private LineRenderer[] _tics;

        private void Start()
        {
            // Do you need this at all?
            _tics = new LineRenderer[numTics];

            var clockFace = new GameObject("Clock Face");
            clockFace.transform.SetParent(transform);

            for (var i = 0; i < numTics; i++)
            {
                var ticObj = new GameObject("Tic " + i);
                ticObj.transform.SetParent(clockFace.transform);

                var ticLR = ticObj.AddComponent<LineRenderer>();
                var meshCollider = ticObj.AddComponent<MeshCollider>();
                ticLR.startWidth = ticWidth;
                ticLR.endWidth = ticWidth;

                // Set to use local space
                ticLR.useWorldSpace = false;
                ticLR.alignment = LineAlignment.TransformZ;
                // Set proper material
                ticLR.material = ticMaterial;



                var angle = i * 360f / numTics;
                var angleInRadians = angle * Mathf.Deg2Rad;

                // Position now in local space!
                var startPos = new Vector3(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians), 0f) * (roseDiameter - ticLength);

                var endPos = startPos + new Vector3(
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    0f
                ) * ticLength;

                // Make sure the position count is correct
                ticLR.positionCount = 2;

                ticLR.SetPositions(new[] { startPos, endPos });

                Mesh mesh = new Mesh();
                ticLR.BakeMesh(mesh, true);
                meshCollider.sharedMesh = mesh;
                meshCollider.enabled = true;

                _tics[i] = ticLR;
            }
            transform.Rotate(90, 0, 0);
        }
        private void Update()
        {
            transform.Rotate(0, 0, anglePerSecond * Time.deltaTime);
        }

        protected void DealDamage(GameObject target)
        {
            target.TryGetComponent(out IHealth damageable);
            damageable?.TakeDamage(_damage);
            Debug.Log("attack from clock");
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                DealDamage(collision.gameObject);

            }

        }
    }
}

