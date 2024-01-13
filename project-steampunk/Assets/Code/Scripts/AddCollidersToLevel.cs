using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class AddCollidersToLevel : MonoBehaviour
{
    /* void Awake()
     {
         GameObject.Find("PlayerPrefab").GetComponent<Rigidbody>().isKinematic = true;
         ApplyChildren_GetComponents(transform.gameObject, AddMeshCollider);
         Debug.Log("childs finished");
         GameObject.Find("PlayerPrefab").GetComponent<Rigidbody>().isKinematic = false;
     }
    */

#if UNITY_EDITOR
    [ContextMenu ("CallInEditor")]
    private void CallInEditor()
    {
        ApplyChildren_GetComponents(transform.gameObject, AddMeshCollider);
    }
    void ApplyChildren_GetComponents(GameObject parent, Action<GameObject> action)
    {
        foreach (Transform child in parent.transform)
        {
            Debug.Log("childs" + child.childCount);
            GameObject go = child.gameObject;
            if (child.transform.childCount == 0)
                action(go);
            else ApplyChildren_GetComponents(go, action);
        }
        /*
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.transform.GetChildCount() < 0)
                action(child.gameObject);
            
        }*/
    }

    void AddMeshCollider(GameObject childObject)
    {
        Mesh mesh = childObject.GetComponent<MeshFilter>().mesh;
        Undo.RecordObject(childObject.AddComponent<MeshCollider>(), "addCollider");
        MeshCollider meshCollider = childObject.AddComponent<MeshCollider>();
        Debug.Log("added collider" + childObject.GetComponent<MeshCollider>());

        if (mesh != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }
#endif
}