using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    public int number;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateScore(int currentNumber)
    {
        number = currentNumber;
    }

    public void ThrowDice()
    {
        float dirX = Random.Range(0, 2000);
        float dirY = Random.Range(0, 2000);
        float dirZ = Random.Range(0, 2000);
        rb.AddForce(rb.transform.TransformDirection(Vector3.up) * 1500);
        rb.AddForce(Vector3.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
