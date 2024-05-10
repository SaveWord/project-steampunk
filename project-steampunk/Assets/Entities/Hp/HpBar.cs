using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    private Camera playerCamera;
    private bool fullHp = true;
    private ITarget player;
    [SerializeField] GameObject _enemyHealthPrefab;
    private void Start()
    {
        playerCamera = Camera.main;
        GetComponentInParent<IHealth>().OnHPChanged += HandleHPSliderChanged;
        GetComponentInParent<IHealth>().OnDied += HandleCloseHpBar;
        player = GetComponentInParent<ITarget>();
        if (player !=null)
            _healthSlider = GetComponentInChildren<Slider>();
    }

    private void FixedUpdate()
    {
       // HpBarRotation();
    }

    private void HandleHPSliderChanged(float currentHp)
    {
        if (fullHp)
        {
            fullHp = false; 
            HandleEnemySliderShow(currentHp);
        }
        _healthSlider.value = currentHp;
    }

    private void HandleEnemySliderShow(float currentHp)
    {
        var hpCanvas = (GameObject)Resources.Load("EnemyHealth", typeof(GameObject));
        if (_enemyHealthPrefab != null)
        {
            _healthSlider = _enemyHealthPrefab.GetComponentInChildren<Slider>();
            _enemyHealthPrefab.SetActive(true);
        }
        
        if (hpCanvas && player == null && _healthSlider==null)
        {  var canvasObject = Instantiate(hpCanvas, new Vector3(playerCamera.transform.position.x,
                    playerCamera.transform.position.y,
                    playerCamera.transform.position.z),
                    UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0)));
                canvasObject.transform.SetParent(this.transform, false);
                canvasObject.transform.localScale = new Vector3(1, 1, 1);
            _healthSlider = GetComponentInChildren<Slider>();
            canvasObject.SetActive(false);
        }
    }
    private void HandleCloseHpBar()
    {
        if(_enemyHealthPrefab!=null)
            _enemyHealthPrefab.SetActive(false);
    }

    private void HpBarRotation()
    {
        if (player == null && _healthSlider)
        {
            _healthSlider.transform.LookAt(_healthSlider.transform.position +
                playerCamera.transform.rotation * Vector3.forward,
                playerCamera.transform.rotation * Vector3.up);
        }
    }
}
