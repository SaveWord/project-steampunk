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
            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                out RaycastHit hitObject, Mathf.Infinity, effectLayer))
            {
                var direction = new Vector3(hitObject.point.x, hitObject.point.y, hitObject.point.z);
                Instantiate(hitEffectPrefab, direction, Quaternion.identity);
                StartCoroutine(recoilShake.Shake(recoilDuration, recoilMagnitude));
            }

            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
                // Instantiate(hitEffectPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);

                //hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                //damageableProps?.GetDamage(damage);

                //hit.collider.TryGetComponent(out IDamageable damageable1);
                //damageable1?.GetDamage(damage);


                ///DealDamage
                hit.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(damage);
            }
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
