using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    // This script handles resizing of the bottom of the cup to fit screen size

    private Vector3 pos;
    private float dist;

    private void Start()
    {
        SetGroundSize();
    }

    private void SetGroundSize()
    {
        // Calculate distance of object's bottom edge from the camera's position
        dist = (transform.position - GameManager.instance.mainCamera.transform.position).y;

        // Convert screen's position to world's position
        pos = GameManager.instance.mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, dist));

        // Set ground size to screen size
        transform.localScale = new Vector3(pos.z, transform.localScale.y, pos.z * 2);
    }
}
