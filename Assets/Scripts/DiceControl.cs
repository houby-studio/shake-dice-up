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
        float dirX = Random.Range(1000, 2000);
        float dirY = Random.Range(1000, 2000);
        float dirZ = Random.Range(1000, 2000);
        rb.AddForce(rb.transform.TransformDirection(Vector3.up) * Random.Range(500, 2000));
        rb.AddForce(Vector3.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
