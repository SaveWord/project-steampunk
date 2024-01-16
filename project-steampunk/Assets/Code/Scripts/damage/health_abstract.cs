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
        healthSlider.value = mineHP;
        if (mineHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if(destructable)
        {
            Destroy(gameObject);
        }
    }
}
