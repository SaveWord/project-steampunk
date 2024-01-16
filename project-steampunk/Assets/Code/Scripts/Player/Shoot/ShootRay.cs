using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootRay : MonoBehaviour
{
    [SerializeField] private Camera cam;
    //[SerializeField] private float maxDistance;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask effectLayer;
    [SerializeField] RecoilShake recoilShake;

    public float recoilDuration;
    public float recoilMagnitude;
    public float damage;
    private Animator animator;
    public GameObject hitEffectPrefab;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward ,
                 Color.red);
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            animator.SetBool("shoot", true);
            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                out RaycastHit hitObject, Mathf.Infinity, effectLayer))
            {
                var direction = new Vector3(hitObject.point.x, hitObject.point.y, hitObject.point.z);
                Instantiate(hitEffectPrefab, direction, Quaternion.identity);
                StartCoroutine(recoilShake.Shake(recoilDuration, recoilMagnitude));
            }

            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                out RaycastHit hit, Mathf.Infinity,enemyLayer))
            {
                // Instantiate(hitEffectPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                hit.collider.TryGetComponent(out IDamageable damageable);
                damageable?.GetDamage(damage);

                hit.collider.TryGetComponent(out enemy_health dama);
                dama?.TakeDamage(damage);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("shoot", false);
        }
    }

}
