using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    event System.Action<float> OnHPChanged;
    event System.Action<float> OnTakenDamage;
    event System.Action<float> OnHealedDamage;
    event System.Action OnDied;
    event System.Action <Vector3> ChangeVfxImpact;
    float CurrentHp { get; }
    void TakeDamage(float amount);
    public void ChangeTransformVFXImpact(Vector3 position);
    void Heal(float amount);
}