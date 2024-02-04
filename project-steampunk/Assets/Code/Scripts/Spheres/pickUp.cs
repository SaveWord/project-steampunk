using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class pickUp : MonoBehaviour
{
    [SerializeField] private string itemTag = "sphere";
    [SerializeField] private float pickupDistance = 15.0f; 
    [SerializeField] private Transform holdPosition;
    //[SerializeField] private GameObject holdP;
    private GameObject heldItem; 

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask shereLayer;
    private bool canDrop = true;
    [SerializeField] private float StartthrowForce = 1f;
    private float throwForce;
    [SerializeField] private float MaxthrowForce = 140f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform throwplayerTransform;
    [SerializeField] private GameObject Firesp;
    [SerializeField] private GameObject ElectSp;
    [SerializeField] private GameObject EarthSp;
    [SerializeField] private GameObject WaterSp;
    private GameObject ChoosenSp;
    //[SerializeField] private GameObject throwplayer;


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
        ChoosenSp = Firesp;
        if (!ChargeOn)
        {
            StartthrowForce = MaxthrowForce;
        }
        throwForce = StartthrowForce;
    }

    public void SwitchFire()
    {
        ChoosenSp = Firesp;
    }
    public void SwitchWater()
    {
        ChoosenSp = WaterSp;
    }
    public void SwitchEarth()
    {
        ChoosenSp = EarthSp;
    }
    public void SwitchElectro()
    {
        ChoosenSp = ElectSp;
    }
    void Update()
    {
       if (hold) {
            if (ChargeOn) { throwForce += 0.08f; }
            
            Debug.Log(throwForce);
            if(throwForce > MaxthrowForce) { throwForce = MaxthrowForce; }
            Vector3 genVelocity = cam.transform.forward * throwForce;
            Vector3 playerPosition = throwplayerTransform.position;
            //ShowTrajectory(playerPosition, genVelocity);
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
        if (context.performed)
        {
            hold = true;
            trajectoryLine.enabled = true;

        }
        else { hold = false; }
        if (context.canceled)
        {
            trajectoryLine.enabled = false;
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = transform.rotation;
            heldItem = Instantiate(ChoosenSp, spawnPosition, spawnRotation);
            DropItem();
        }

        //RaycastHit hit;
        //if (AimedAt && !HandsBusy)
        //{
        //    Debug.Log("hilding nothing");
        //    if (context.canceled)
        //    {
        //        AimedAt.GetComponent<Rigidbody>().isKinematic = true;
        //        AimedAt.transform.SetParent(holdPosition);
        //        AimedAt.transform.localPosition = Vector3.zero;
        //        canDrop = false;
        //        StartCoroutine(EnableDropDelay(0.5f));
        //        blowUp myScriptReference = AimedAt.GetComponent<blowUp>();
        //        if (myScriptReference != null)
        //        {
        //            myScriptReference.Blow();
        //        }
        //        HandsBusy = true;
        //        heldItem = AimedAt;
        //    }

        //}
        //else if (canDrop && HandsBusy)
        //{
        //    if (context.performed)
        //    {
        //        hold = true;
        //        trajectoryLine.enabled = true;

        //    }
        //    else { hold = false; }
        //    if (context.canceled)
        //    {
        //        trajectoryLine.enabled = false;
        //        DropItem();
        //    }

        //}
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(throwplayerTransform);
            heldItem.transform.localPosition = Vector3.zero;
            if (throwForce > MaxthrowForce) { throwForce = MaxthrowForce; }
            //if (!ChargeOn) { throwForce = MaxthrowForce; }
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            //heldItem.transform.localScale = new Vector3(0.806f, 0.712f, 0f);
            //heldItem.transform.SetParent(throwplayerTransform);
            //heldItem.transform.localScale = new Vector3(0.806f, 0.712f, 0f);
            heldItem.transform.SetParent(null);
            //Debug.Log("hERE");
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
