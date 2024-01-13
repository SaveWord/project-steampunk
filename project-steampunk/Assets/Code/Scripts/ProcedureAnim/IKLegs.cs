using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegs : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKLegs otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    [SerializeField] private Type legType;
    [SerializeField] Transform legRay;
    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    public enum Type
    {
        legRightBackward,
        legLeftBackward,
        legRightForward,
        legLeftForward,
    }
    private void Start()
    {
        footSpacing = transform.localPosition.z;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
        legRay.position = transform.position;
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        //Vector3 right = Vector3.zero;
        //Ray ray = new Ray(body.position + (right * footSpacing), Vector3.down); //body.right
       /* switch (legType)
        {
            case Type.legLeftForward:
                // right = new Vector3(-body.right.x, body.right.y, body.right.z);
                //ray = new Ray(legRay.position, Vector3.down);
                break;
            case Type.legRightForward:
                //right = new Vector3(body.right.x, body.right.y, body.right.z);
                //ray = new Ray(legRay.position, Vector3.down);
                break;
            case Type.legLeftBackward:
                // right = new Vector3(-body.right.x, body.right.y, -body.right.z);
                //ray = new Ray(legRay.position, Vector3.down);
                break;
            case Type.legRightBackward:
                //right = new Vector3(body.right.x, body.right.y, -body.right.z);
                //ray = new Ray(legRay.position, Vector3.down);
                break;
        }*/

        Ray ray = new Ray();
        ray = new Ray(legRay.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }



    public bool IsMoving()
    {
        return lerp < 1;
    }

}
