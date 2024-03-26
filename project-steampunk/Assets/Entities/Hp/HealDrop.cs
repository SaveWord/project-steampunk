using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDrop : MonoBehaviour
{
    private float _healAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Healed"+ other.name);
        if (other.gameObject.layer == 7)
            PickUpHeal(other.gameObject);
    }

    void PickUpHeal(GameObject player)
    {
        player.GetComponent<IHealth>().Heal(_healAmount);
        Destroy(gameObject);
    }

}
