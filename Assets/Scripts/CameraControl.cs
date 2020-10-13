using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private void Start()
    {
        if (!Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
        }
    }

    private void Update()
    {
        transform.rotation = Input.gyro.attitude;
    }
}
