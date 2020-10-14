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

    private Rigidbody rigid;

    private void Start()
    {
        if (!Input.gyro.enabled)
            Input.gyro.enabled = true;
    }

    private void Update()
    {
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = DeviceRotation.Get();
        if (isFlat)
            deviceRotation = Quaternion.Euler(0, 0, 0) * deviceRotation;
        //Quaternion eliminationOfXY = Quaternion.Inverse(
        //    Quaternion.FromToRotation(referenceRotation * Vector3.forward,
        //                              deviceRotation * Vector3.forward)
        //);
        //Quaternion rotationZ = eliminationOfXY * deviceRotation;

        transform.rotation = deviceRotation;
        //float roll = rotationZ.eulerAngles.z;
        //transform.rotation = Input.gyro.attitude;
    }
}
