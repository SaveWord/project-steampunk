using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    event System.Action<float> OnHPChanged;
    float CurrentHp { get; }
    void TakeDamage(int amount);
    void Heal(int amount);
}