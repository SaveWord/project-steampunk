using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarrelExplosion : MonoBehaviour,IDamageableProps
{
    [SerializeField] private float damage;
    [SerializeField] private float rangeExplosion;
    [SerializeField] private float propsHP;
    private ParticleSystem explosionParticle;
    public Slider sliderHP;
    private Canvas canvas;
    private  void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        sliderHP.value = propsHP;
        sliderHP.gameObject.SetActive(false);
        explosionParticle = GetComponentInChildren<ParticleSystem>();
    }


    //get damage and if die give damage
    private void GiveDamageIfDie()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangeExplosion);
        for (int i =0; i < hitColliders.Length-1; i++)
        {
            if (hitColliders[i].CompareTag("enemy") || hitColliders[i].CompareTag("Player"))
            {
                hitColliders[i].TryGetComponent(out IDamageable damageable);
                damageable.GetDamage(damage);
            }
        }
    }
    public  void GetDamage(float damage)
    {
        propsHP -= damage;
        sliderHP.value = propsHP;
        StartCoroutine(HPView());
        if (propsHP <= 0)
        {
            GiveDamageIfDie();
            StartCoroutine(DestroyBarrel());
        }
    }
    IEnumerator DestroyBarrel()
    {
        explosionParticle.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private IEnumerator HPView()
    {
        sliderHP.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        sliderHP.gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Color tmp = Color.red;
        tmp.a = 0.5f;
        Gizmos.color = tmp;
        Gizmos.DrawSphere(transform.position, rangeExplosion);
    }
}
