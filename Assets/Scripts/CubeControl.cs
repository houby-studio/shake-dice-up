using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
    private static Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            float dirX = Random.Range(0, 1000);
            float dirY = Random.Range(0, 1000);
            float dirZ = Random.Range(0, 1000);
            rb.AddForce(transform.up * 1000);
            rb.AddTorque(dirX, dirY, dirZ);
            rb.AddForce(transform.up * 1000);
        }
    }
}
