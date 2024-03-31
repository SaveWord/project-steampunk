using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : MonoBehaviour, IShield
{
    [SerializeField] private float disolveRate;
    [SerializeField] private float refreshRate;
    private Material m_Material;

    private void Start()
    {
        m_Material = GetComponent<Renderer>().material;
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
        Destroy(gameObject);
    }
}
