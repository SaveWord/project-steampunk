using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class pickUp : MonoBehaviour
{
    [SerializeField] private string itemTag = "sphere";
    [SerializeField] private float pickupDistance = 15.0f; 
    [SerializeField] private Transform holdPosition; 
    private GameObject heldItem; 

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask shereLayer;
    private bool canDrop = true;
    [SerializeField] private float StartthrowForce = 1f;
    private float throwForce;
    [SerializeField] private float MaxthrowForce = 140f;
    [SerializeField] private Transform playerTransform;


    private bool isCharging = false;
    private float chargingTime = 0f;

    private bool hold = false;
    [SerializeField] private LineRenderer trajectoryLine;


    Vector3 origin;
    Vector3 direction;
    float maxDistance = 15f;
    //int layerMask = DefaultRaycastLayers;
    QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;

    [SerializeField] private int AimRadiusIterations = 70;
    float initialRadius = 0.01f;
    Outline script;
    private GameObject AimedAt;
    private bool HandsBusy = false;

    [SerializeField] private bool ChargeOn;

    private void Start()
    {
        if (!ChargeOn)
        {
            StartthrowForce = MaxthrowForce;
        }
        throwForce = StartthrowForce;
    }
    void Update()
    {
       if (hold) {
            if (ChargeOn) { throwForce += 0.08f; }
            
            Debug.Log(throwForce);
            if(throwForce > MaxthrowForce) { throwForce = MaxthrowForce; }
            Vector3 genVelocity = cam.transform.forward * throwForce;
            Vector3 playerPosition = playerTransform.position;
            ShowTrajectory(playerPosition, genVelocity);
       }

        origin = cam.transform.position;
        direction = cam.transform.forward;

        for (int i = 0; i < AimRadiusIterations; i++)
        {
            if (script != null)
            {
                script.Turnoff();
            }
            AimedAt = null;


            float radius = initialRadius * (i + 0.1f); 

            RaycastHit hit;

            if (Physics.SphereCast(origin, radius, direction, out hit, maxDistance, shereLayer, queryTriggerInteraction))
            {
                //Debug.Log($"SphereCast hit at radius {radius}: {hit.collider.gameObject.name}");
                script = hit.collider.gameObject.GetComponent<Outline>();
                AimedAt = hit.collider.gameObject;
                if (script != null)
                {
                    script.Turnon();
                }
                break;
            }
            else
            {
                if (script != null)
                {
                    script.Turnoff();
                }
                AimedAt = null;
                //Debug.Log($"No collision at radius {radius}");
            }
        }

    }
    void ShowTrajectory(Vector3 playerPos, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        trajectoryLine.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
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
        if (AimedAt&&!HandsBusy)
        {
            Debug.Log("hilding nothing");
            if (context.canceled)
            {
                AimedAt.GetComponent<Rigidbody>().isKinematic = true;
                AimedAt.transform.SetParent(holdPosition);
                AimedAt.transform.localPosition = Vector3.zero;
                canDrop = false;
                StartCoroutine(EnableDropDelay(0.5f));
                blowUp myScriptReference = AimedAt.GetComponent<blowUp>();
                if (myScriptReference != null)
                {
                    myScriptReference.Blow();
                }
                HandsBusy = true;
                heldItem = AimedAt;
            }
            //AimedAt.GetComponent<Rigidbody>().isKinematic = true;
            //AimedAt.transform.SetParent(holdPosition);
            //AimedAt.transform.localPosition = Vector3.zero;
            //canDrop = false;
            //StartCoroutine(EnableDropDelay(0.5f));
            //blowUp myScriptReference = AimedAt.GetComponent<blowUp>();
            //if (myScriptReference != null)
            //{
            //    myScriptReference.Blow();
            //}
            //HandsBusy = true;
            //heldItem = AimedAt;

        }
        else if(canDrop&& HandsBusy)
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
            //if (!ChargeOn) { throwForce = MaxthrowForce; }
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.transform.SetParent(null);
            Debug.Log("hERE");
            heldItem.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);

            heldItem = null;
            HandsBusy = false;
            throwForce = StartthrowForce;
        }
    }

    IEnumerator EnableDropDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDrop = true; // Enable dropping after the specified delay
    }
}
