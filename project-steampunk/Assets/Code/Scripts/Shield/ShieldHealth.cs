using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : MonoBehaviour, IShield
{
    [SerializeField] private float disolveRate;
    [SerializeField] private float refreshRate;
    [SerializeField] private bool isWall = false;
    private Material m_Material;
     private GameObject healDropPrefab;

    private void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }
    void OnEnable()
    {
        m_Material = GetComponent<Renderer>().material;
        m_Material.SetFloat("_DisolveAmount", 0f);
    }
    public void ShieldImpulse()
    {
        StartCoroutine(ShieldImpulseVFX());
    }
    IEnumerator ShieldImpulseVFX()
    {

        m_Material.SetFloat("_FreshelPower", 2.2f);
        m_Material.SetVector("_VertexAmount", new Vector3(0.05f, 0.05f, 0.05f));
        yield return new WaitForSeconds(refreshRate);
        m_Material.SetFloat("_FreshelPower", 2f);
        m_Material.SetVector("_VertexAmount", new Vector3(0, 0, 0));

        /*
        float counter = 0;
        while(m_Material.GetFloat("_FreshelPower") <= 2 &&
            m_Material.GetFloat("_FreshelPower") >= 1.8f)
        {
            counter += 0.2f;
            m_Material.SetFloat("_FreshelPower", counter);
            m_Material.SetVector("_VertexAmount",new Vector3(0.05f,0.05f,0.05f));
            yield return new WaitForSeconds(refreshRate);
        }
        while(m_Material.GetFloat("_FreshelPower") >= 2 &&
            m_Material.GetFloat("_FreshelPower") <= 2.2f)
        {
            counter -= 0.2f;
            m_Material.SetFloat("_FreshelPower", counter);
            m_Material.SetVector("_VertexAmount", new Vector3(0, 0, 0));
            yield return new WaitForSeconds(refreshRate);
        }
       */


    }
    public void ShieldDestroy()
    {
        if (isWall)
        {
            healDropPrefab = Resources.Load<GameObject>("HealDrop");

           
            // drop the heals
            var healCount = UnityEngine.Random.Range(0, 2);
            Debug.Log("Healed num " + healCount);
            for (int i = 0; i <= healCount; i++)
            {

                var position = new Vector3(transform.position.x + UnityEngine.Random.Range(-10, 10), transform.position.y, transform.position.z + UnityEngine.Random.Range(-10, 10));

                Instantiate(healDropPrefab, position, Quaternion.identity);
            }
        }
        StartCoroutine(ShieldDestroyVFX());
        
    }
    IEnumerator ShieldDestroyVFX()
    {
        float counter = 0;
        while (m_Material.GetFloat("_DisolveAmount") < 1)
        {
            counter += disolveRate;
            m_Material.SetFloat("_DisolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
        if(isWall)
            transform.parent.gameObject.SetActive(false);
    }
}
