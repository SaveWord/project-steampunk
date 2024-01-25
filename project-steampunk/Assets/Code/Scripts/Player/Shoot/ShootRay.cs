using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShootRay : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask effectLayer;
    [SerializeField] RecoilShake recoilShake;

    [SerializeField] private int maxPatrons;
    [SerializeField] private float waitBeforeReload;

    private int patrons;
    private TextMeshProUGUI patronsText;

    public float recoilDuration;
    public float recoilMagnitude;
    public float damage;
    private Animator animatorWeapon;
    private Animator animatorRightArm;
    public GameObject hitEffectPrefab;
    [SerializeField] private ParticleSystem deathParticlePrefab;

    private void Start()
    {
        patrons = maxPatrons;
        animatorWeapon = GetComponent<Animator>();
        animatorRightArm = transform.root.GetComponentInChildren<Animator>();
        patronsText = transform.root.GetComponentInChildren<TextMeshProUGUI>();
        patronsText.text = patrons.ToString();
    }

    private void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward,
                 Color.red);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started && patrons > 0)
        {
            patrons--;
            patronsText.text = patrons.ToString();
            animatorWeapon.SetBool("shoot", true);//анимация поворота барабана и курка
            ShootRaycast();
        }
        if (patrons == 0)
        {
            Reload(context);
        }
        if (context.canceled)
        {
            animatorWeapon.SetBool("shoot", false);//анимация поворота барабана и курка
        }
    }

    private void ShootRaycast()
    {
       
        if (Physics.Raycast(cam.transform.position, cam.transform.forward,
            out RaycastHit hitObject, Mathf.Infinity))
        {
            if (((1 << hitObject.transform.gameObject.layer) & enemyLayer) != 0)
            {
                // DealDamage
                hitObject.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(damage);
                HitSuccessfulParticle(hitObject.transform.gameObject);
            }
            else
            {
                var direction = new Vector3(hitObject.point.x, hitObject.point.y, hitObject.point.z);
                HitParticle(direction);
            }
        }
        
    }
    private void HitSuccessfulParticle(GameObject point)
    {
        var deathparticle = Instantiate(deathParticlePrefab, point.transform.position, point.transform.rotation);
        Destroy(deathparticle, 2f);
    }

    private void HitParticle(Vector3 direction)
    {
        // Instantiate(hitEffectPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
        Instantiate(hitEffectPrefab, direction, Quaternion.identity);
        StartCoroutine(recoilShake.Shake(recoilDuration, recoilMagnitude));
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.started && patrons < maxPatrons)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        animatorWeapon.SetBool("shoot", false);
        animatorWeapon.SetBool("reload", true);
        yield return new WaitForSeconds(waitBeforeReload);
        animatorWeapon.SetBool("reload", false);
        patrons = maxPatrons;
        patronsText.text = patrons.ToString();
    }

}
