using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public static class DeviceRotation
{
    private static bool gyroInitialized = false;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion Get()
    {
        if (!gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroscope
            ? ReadGyroscopeRotation()
            : Quaternion.identity;
    }

    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        }
        gyroInitialized = true;
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}

public class CameraControl : MonoBehaviour
{
    public bool isFlat = true;

    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the
    // filtered value will converge towards current input sample (and vice versa).
    private float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation,
    // or at least according to Brady! ;)
    private float shakeDetectionThreshold = 2.0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;

    private Rigidbody rigid;

    private void Start()
    {
        if (!Input.gyro.enabled)
            Input.gyro.enabled = true;

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    private void Update()
    {
        // Shake detection
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            // Perform your "shaking actions" here. If necessary, add suitable
            // guards in the if check above to avoid redundant handling during
            // the same shake (e.g. a minimum refractory period).
            //Debug.Log("Shake event detected at time " + Time.time);
        }
        
        //// Rotate cup
        //Quaternion referenceRotation = Quaternion.identity;
        //Quaternion deviceRotation = DeviceRotation.Get();
        //if (isFlat)
        //    deviceRotation = Quaternion.Euler(0, 0, 0) * deviceRotation;
        ////transform.rotation = deviceRotation; -- THIS WORKS SOMEWHAT


        Vector3 previousEulerAngles = transform.eulerAngles;
        Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

        Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
        //targetEulerAngles.x = 0.0f; // Only this line has been added
        targetEulerAngles.z = 0.0f; // up and down
        targetEulerAngles.y = 0.0f; // up and down
        transform.eulerAngles = targetEulerAngles;
        transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
    }
}
