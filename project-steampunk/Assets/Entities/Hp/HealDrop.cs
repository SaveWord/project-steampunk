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

    void Awake()
    {
        var m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = new Vector3(0, -_speed, 0);
        Destroy(gameObject, _healDestroyTime);
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
