using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenEdge
{
    Top, Bottom, Left, Right
}

public class Walls : MonoBehaviour
{
    // Should object stick to Top, Bottom, Left or Right screen's edge?
    public ScreenEdge screenEdge;

    private Camera cam;
    private Vector3 pos;
    private float dist;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        setWallsPosition();
    }

    private void setWallsPosition()
    {
        // Calculate distance of object's bottom edge from the camera's position
        dist = ((transform.position - transform.localScale / 2) - cam.transform.position).y;

        // Convert screen's position to world's position
        pos = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, dist));

        // Set position and scale object to selected screen's edge
        switch (screenEdge)
        {
            case ScreenEdge.Top:
                transform.position = new Vector3(transform.position.x, transform.position.y, pos.z);
                transform.localScale = new Vector3(pos.x * 2, transform.localScale.y, transform.localScale.z);
                break;
            case ScreenEdge.Bottom:
                transform.position = new Vector3(transform.position.x, transform.position.y, -pos.z);
                transform.localScale = new Vector3(pos.x * 2, transform.localScale.y, transform.localScale.z);
                break;
            case ScreenEdge.Left:
                transform.position = new Vector3(-pos.x, transform.position.y, transform.position.z);
                transform.localScale = new Vector3(pos.z * 2, transform.localScale.y, transform.localScale.z);
                break;
            case ScreenEdge.Right:
                transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
                transform.localScale = new Vector3(pos.z * 2, transform.localScale.y, transform.localScale.z);
                break;
        }
    }
}
