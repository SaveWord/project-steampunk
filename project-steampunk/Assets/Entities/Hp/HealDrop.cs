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
    ParticleSystem.MainModule ps;
    private ParticleSystem.MinMaxGradient originalColor;
    void Awake()
    {
        var m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = new Vector3(0, -_speed, 0);
        

        ps = GetComponentInChildren<ParticleSystem>().main;
        Destroy(gameObject, _healDestroyTime);
        originalColor = ps.startColor;
        InvokeRepeating("ChangeOpacity", _healDestroyTime/3 , 0.3f);
        InvokeRepeating("ChangeColor", _healDestroyTime / 3 + 0.15f, 0.3f);

    }
    private void ChangeOpacity()
    {
       ps.startColor = new Color(13, 180, 0, 0);
    }
    private void ChangeColor()
    {
        ps.startColor = originalColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == 7)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, other.gameObject.transform.position, _speed * Time.deltaTime);
           
            PickUpHeal(other.gameObject);  Debug.Log("Healed"+ other.name);
        }
    }

    void PickUpHeal(GameObject player)
    {
        player.GetComponent<IHealth>().Heal(_healAmount);
        Destroy(gameObject);
    }

}
