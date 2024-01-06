using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class pickUp : MonoBehaviour
{
    [SerializeField] private string itemTag = "sphere";
    [SerializeField] private float pickupDistance = 15.0f; 
    [SerializeField] private Transform holdPosition; 
    private GameObject heldItem; 

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask shereLayer;
    private bool canDrop = true;
    [SerializeField] private float throwForce = 4f;
    [SerializeField] private float MaxthrowForce = 140f;
    [SerializeField] private Transform playerTransform;


    private bool isCharging = false;
    private float chargingTime = 0f;

    private bool hold = false;
    [SerializeField] private LineRenderer trajectoryLine;
    void Update()
    {
       if (hold) {
            throwForce += 0.08f;
            Debug.Log(throwForce);
            if(throwForce > MaxthrowForce) { throwForce = MaxthrowForce; }
            Vector3 genVelocity = cam.transform.forward * throwForce;
            Vector3 playerPosition = playerTransform.position;
            ShowTrajectory(playerPosition, genVelocity);
       }
    }
    void ShowTrajectory(Vector3 playerPos, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = playerPos + speed * time + 0.5f * Physics.gravity * time * time;
        }
        trajectoryLine.SetPositions(points);
    }
    //InputAction.CallbackContext context
    public void PickIt(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (!heldItem)
        {
            //Debug.Log("hilding nothing");
            
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance, shereLayer))
            {
                //Debug.Log("hit layer");
                Debug.DrawRay(cam.transform.position, cam.transform.forward * pickupDistance, Color.red);
                if (hit.collider.CompareTag(itemTag))
                {
                    //Debug.Log("phase " + context.phase);
                    heldItem = hit.collider.gameObject;
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                    heldItem.transform.SetParent(holdPosition);
                    heldItem.transform.localPosition = Vector3.zero;
                    canDrop = false;
                    StartCoroutine(EnableDropDelay(0.5f));

                    blowUp myScriptReference = heldItem.GetComponent<blowUp>();
                    if (myScriptReference != null)
                    {
                        myScriptReference.Blow();
                    }
                }
            }
        }
        else if(canDrop)
        {
            Debug.Log("phase " + context.phase);
            if (context.performed)
            {
                hold = true;
                trajectoryLine.enabled = true;
            }
            else { hold = false; }
            if (context.canceled)
            {
                trajectoryLine.enabled = false;
                DropItem();
            }
            
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            //throwForce = Mathf.Min(throwForce, MaxthrowForce);
            if (throwForce > MaxthrowForce) { throwForce = MaxthrowForce; }
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.transform.SetParent(null);
            heldItem.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);

            heldItem = null;
            throwForce = 1f;
        }
    }

    IEnumerator EnableDropDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDrop = true; // Enable dropping after the specified delay
    }
}
