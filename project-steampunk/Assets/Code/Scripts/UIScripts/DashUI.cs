using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    private CharacterControllerMove playerDash;
    private Slider slider;
    private void OnEnable()
    {
        playerDash = transform.root.GetComponent<CharacterControllerMove>();
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        slider.value = playerDash.DashSlider;
    }
}
