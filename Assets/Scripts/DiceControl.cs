using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{

    // This script controls the dice physics and allows dice to be thrown and be frozen

    public int number;
    public float fallMultiplier; // How much faster does object fall to the ground
    public bool frozen = false;

    private Rigidbody rb;
    private Camera cam;
    private Outline outline;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        cam = Camera.main;
        UpdateOutlineColor();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -1)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        if (rb.velocity.magnitude > GameManager.instance.maxSpeed)
            rb.velocity = rb.velocity.normalized * GameManager.instance.maxSpeed;
    }

    public void UpdateScore(int currentNumber)
    {
        number = currentNumber;
    }

    public void ThrowDice()
    {
        if (!frozen)
        {
            float dirX = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
            float dirY = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
            float dirZ = (Random.Range(0, 2) * 2 - 1) * Random.Range(1000, 2000);
            rb.AddForce(rb.transform.TransformDirection(Vector3.up) * Random.Range(500, 2000));
            rb.AddForce((cam.transform.position - rb.position) * 200);
            rb.AddTorque(dirX, dirY, dirZ);
        }
    }

    public void ToggleFreeze()
    {
        if (rb.velocity.magnitude < GameManager.instance.freezableMagnitudeLimit)
        { 
            frozen = !frozen;
            outline.enabled = !outline.enabled;
            rb.freezeRotation = !rb.freezeRotation;
            GameManager.instance.UpdateButtonFunction();
        }
    }

    public void UpdateOutlineColor()
    {
        outline.OutlineColor = GameManager.instance.dotMaterial.color;
    }
}
