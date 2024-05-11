using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    private CharacterControllerMove playerDash;
    private Slider slider;
    private CanvasGroup canvasGroupHide;
    private void Start()
    {
        playerDash = transform.root.GetComponent<CharacterControllerMove>();
        slider = GetComponent<Slider>();
        canvasGroupHide = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        slider.value = playerDash.DashSlider;
        if (playerDash.DashSlider == 1) canvasGroupHide.alpha = 0;
        else canvasGroupHide.alpha = 1;
    }
}
