using UnityEngine;
using System.Collections;

public class pickUp : MonoBehaviour
{
    [SerializeField] private string itemTag = "sphere";
    [SerializeField] private float pickupDistance = 15.0f; 
    [SerializeField] private Transform holdPosition; 
    private GameObject heldItem; 

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask shereLayer;
    private bool canDrop = true;
    [SerializeField] private float throwForce = 40f;
    void Update()
    {
       
    }

    public void PickIt()
    {
        RaycastHit hit;
        if (!heldItem)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance, shereLayer))
            {
                Debug.DrawRay(cam.transform.position, cam.transform.forward * pickupDistance, Color.red);
                if (hit.collider.CompareTag(itemTag))
                {
                    heldItem = hit.collider.gameObject;
                    var itemInStash = Instantiate(heldItem, new Vector3(heldItem.transform.position.x, heldItem.transform.position.y, heldItem.transform.position.z), Quaternion.identity);
                    itemInStash.transform.SetParent(GameObject.Find("Spheres").transform);
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
            DropItem();
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.transform.SetParent(null);
            heldItem.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
            heldItem = null;
        }
    }

    IEnumerator EnableDropDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDrop = true; // Enable dropping after the specified delay
    }
}
