using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] LegStepper frontLeftLegStepper;
    [SerializeField] LegStepper frontRightLegStepper;
    [SerializeField] LegStepper backLeftLegStepper;
    [SerializeField] LegStepper backRightLegStepper;
    private void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
    }
    IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            do
            {
                frontLeftLegStepper.TryToMove();
                backRightLegStepper.TryToMove();
                yield return null;
            } while (backRightLegStepper.moving || frontLeftLegStepper.moving);

            do
            {
                frontRightLegStepper.TryToMove();
                backLeftLegStepper.TryToMove();
                yield return null;
            } while (backLeftLegStepper.moving || frontRightLegStepper.moving);
        }
    }
}
