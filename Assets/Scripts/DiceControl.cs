using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    public int number;
    public float fallMultiplier;

    private Rigidbody rb;
    private Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -1)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        if (rb.velocity.magnitude > GameManager.instance.maxSpeed)
            rb.velocity = rb.velocity.normalized * GameManager.instance.maxSpeed;
        //if (rb.)
        //    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    public void UpdateScore(int currentNumber)
    {
        number = currentNumber;
    }

    public void ThrowDice()
    {
        float dirX = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
        float dirY = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
        float dirZ = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
        rb.AddForce(rb.transform.TransformDirection(Vector3.up) * Random.Range(500, 2000));
        rb.AddForce((cam.transform.position - rb.position) * 200);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
