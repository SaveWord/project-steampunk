using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealDrop : MonoBehaviour
{
    public float _healAmount = 5;
    private float _speed = 30f;
    public float _healDestroyTime = 10f;
    ParticleSystem.MainModule _healParticleSystem;
    private ParticleSystem.MinMaxGradient _healOriginalColor;
    void Awake()
    {
       // var m_Rigidbody = GetComponent<Rigidbody>();
      // m_Rigidbody.velocity = new Vector3(0, -_speed, 0);
        

        _healParticleSystem = GetComponentInChildren<ParticleSystem>().main;
        Destroy(gameObject, _healDestroyTime);
        _healOriginalColor = _healParticleSystem.startColor;
        InvokeRepeating("ChangeOpacity", _healDestroyTime/3 , 0.3f);
        InvokeRepeating("ChangeColor", _healDestroyTime / 3 + 0.15f, 0.3f);

    }
    private void ChangeOpacity()
    {
       _healParticleSystem.startColor = new Color(13, 180, 0, 0);
    }
    private void ChangeColor()
    {
        _healParticleSystem.startColor = _healOriginalColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //player
            PickUpHeal(other.gameObject);  Debug.Log("Healed"+ other.name);
    }

    void PickUpHeal(GameObject player)
    {
        var playerHealth = player.GetComponent<HpHandler>();
        if (playerHealth.CurrentHp < playerHealth.MaxHp)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.transform.position, _speed * Time.deltaTime);
            playerHealth.Heal(_healAmount);
            Destroy(gameObject);
        }
    }

}
