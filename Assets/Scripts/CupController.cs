using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    public float maxSpeed;

    private Quaternion Rotation_Origin;
    private Gyroscope Gyroscope_Reference;

    private void Awake()
    {
        Gyroscope_Reference = Input.gyro;
        Gyroscope_Reference.enabled = true;
        StartCoroutine(Coroutine_Method());
    }

    private IEnumerator Coroutine_Method()
    {
        yield return null;
        Quaternion Rotation_Origin_Addend = Quaternion.Euler(0, 0, 180);
        Rotation_Origin = Gyroscope_Reference.attitude * Rotation_Origin_Addend;

        Quaternion Gyroscope_Attitude_Difference_Addend = Quaternion.Euler(180, 180, 0);

        while (true)
        {
            Quaternion Gyroscope_Attitude_Difference = Quaternion.Inverse(Rotation_Origin) * Gyroscope_Reference.attitude;
            Gyroscope_Attitude_Difference *= Gyroscope_Attitude_Difference_Addend;

            Quaternion Lerped_Quaternion = Quaternion.Lerp(transform.rotation, Gyroscope_Attitude_Difference, maxSpeed * Time.deltaTime);
            // Limit max X angle
            if (Lerped_Quaternion.x > 0.1f)
                Lerped_Quaternion.x = 0.1f;
            else if (Lerped_Quaternion.x < -0.1f)
                Lerped_Quaternion.x = -0.1f;
            // Z and Y not interested
            Lerped_Quaternion.z = 0;
            Lerped_Quaternion.y = 0;

            transform.rotation = Lerped_Quaternion;

            //transform.Rotate(0f, 0f, 180f, Space.Self);
            //transform.Rotate(-180f, 180f, 0, Space.World);
            yield return null;
        }
    }

}
