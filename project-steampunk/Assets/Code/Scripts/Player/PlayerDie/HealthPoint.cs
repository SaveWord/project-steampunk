using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthPoint : MonoBehaviour, IDamageable
{
    public bool parry;
    [SerializeField] private float parryTime;
    [SerializeField] Image hpUI;
    [SerializeField] private float hp;
    [SerializeField] private float timeIntervalAttack;
    private float timeNextAttack;
    [SerializeField] private DamageTakenEffect damageEffect;

    public void GetDamage(float damage)
    {
        if (timeNextAttack < Time.time)
        {
            timeNextAttack = Time.time + timeIntervalAttack;
            hp -= damage;
            StartCoroutine(damageEffect.TakeDamageEffect());
            hpUI.fillAmount = hp;
            if (hp <= 0.01)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }


    public void Status(string state)
    {

    }
}
