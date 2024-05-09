using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISliderTuner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _valueShift;


    public void OnPointerClick(PointerEventData eventData)
    {
        _slider.value += _valueShift;
    }

}
