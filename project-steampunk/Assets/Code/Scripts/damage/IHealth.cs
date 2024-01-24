using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    event System.Action<float> OnHPChanged;
    event System.Action OnDied;
    float CurrentHp { get; }
    void TakeDamage(float amount);
    void Heal(float amount);
}