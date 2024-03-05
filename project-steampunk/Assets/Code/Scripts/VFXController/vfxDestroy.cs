using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class vfxDestroy : MonoBehaviour
{
    [SerializeField] private float vfxTimeDestroy = 1f;
    private DecalProjector projector;
    private void Awake()
    {
        projector = GetComponentInChildren<DecalProjector>();
        StartCoroutine(VFXDestroy());
    }
    IEnumerator VFXDestroy()
    {
        InvokeRepeating("OpacityDecalChange", 0,0.1f);
        yield return new WaitForSeconds(vfxTimeDestroy);
        GameObject.Destroy(gameObject);
    }
    private void OpacityDecalChange()
    {
        projector.fadeFactor -= 0.01f ; 
    }
}
