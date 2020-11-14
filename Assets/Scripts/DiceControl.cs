using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DiceControl : MonoBehaviour
{

    // This script controls the dice physics and allows dice to be thrown and be frozen

    public int number;
    public bool frozen = false;

    private Rigidbody rb;
    private Outline outline;
    private AudioSource hitSound;

    void Start()
    {
        if (gameObject.tag != "Preload")
        {
            rb = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();
            hitSound = GetComponent<AudioSource>();
            UpdateOutlineColor();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hitSound.isPlaying && hitSound.isActiveAndEnabled)
            hitSound.Play();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -1)
            rb.velocity += Vector3.up * Physics.gravity.y * (GameManager.instance.fallMultiplierSpeed - 1) * Time.deltaTime;
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
            rb.AddForce((GameManager.instance.mainCamera.transform.position - rb.position) * 200);
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

    public void Unfreeze()
    {
        frozen = false;
        outline.enabled = false;
        rb.freezeRotation = false;
    }

    public void UpdateOutlineColor()
    {
       outline.OutlineColor = GameManager.instance.dotMaterial.color;
    }
}
