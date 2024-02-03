using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar: MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    private Camera playerCamera;
    private bool fullHp = true;
    private void Start()
    {
        playerCamera = Camera.main;
        GetComponentInParent<IHealth>().OnHPChanged += HandleHPSliderChanged;
        var player = GetComponentInParent<ITarget>();
        if (player !=null)
            _healthSlider = GetComponentInChildren<Slider>();
    }

    private void FixedUpdate()
    {
        HpBarRotation();
    }

    private void HandleHPSliderChanged(float currentHp)
    {
        if (fullHp)
        {
            fullHp = false; 
            HandleEnemySliderShow();
        }
        _healthSlider.value = currentHp;
    }

    private void HandleEnemySliderShow()
    {
        var hpCanvas = (GameObject)Resources.Load("EnemyHealth", typeof(GameObject));
        if (hpCanvas)
        {
            var canvasObject = Instantiate(hpCanvas, new Vector3(playerCamera.transform.position.x,
                playerCamera.transform.position.y,
                playerCamera.transform.position.z),
                UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0)));
            canvasObject.transform.SetParent(this.transform, false);
            canvasObject.transform.localScale = new Vector3(1, 1, 1);
            _healthSlider = GetComponentInChildren<Slider>();
        }
    }
    private void HpBarRotation()
    {
        if (!fullHp && _healthSlider)
        {
            _healthSlider.transform.LookAt(_healthSlider.transform.position +
                playerCamera.transform.rotation * Vector3.forward,
                playerCamera.transform.rotation * Vector3.up);
        }
    }
}
