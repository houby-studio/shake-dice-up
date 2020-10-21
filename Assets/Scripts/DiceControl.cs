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
        float dirX = Random.Range(0, 1000);
        float dirY = Random.Range(0, 1000);
        float dirZ = Random.Range(0, 1000);
        rb.AddForce(transform.up * 1000);
        rb.AddTorque(dirX, dirY, dirZ);
        rb.AddForce(transform.up * 1000);
    }
}
