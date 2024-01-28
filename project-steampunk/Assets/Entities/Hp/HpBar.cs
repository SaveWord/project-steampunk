using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar: MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    private void Start()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        GetComponentInParent<IHealth>().OnHPChanged += HandleHPSliderChanged;
    }

    private void HandleHPSliderChanged(float currentHp)
    {
        _healthSlider.value = currentHp;
    }
}
