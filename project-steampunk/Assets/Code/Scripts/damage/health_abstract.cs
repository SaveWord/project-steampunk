using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class health_abstract: MonoBehaviour
{
    [SerializeField] protected float mineHP;
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected bool destructable;

    protected void UpdateHealth()
    {
        healthSlider.value = mineHP/100;
        if (mineHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(destructable)
        {
            Destroy(gameObject);

        }
    }
    public virtual void TakeDamage(float dama)
    {

    }
}
