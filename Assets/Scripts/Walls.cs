using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public Vector3 side;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        setWallsPosition();
    }

    void Update()
    {
        
    }

    private void setWallsPosition()
    {
        float dist = ((transform.position - transform.localScale / 2) - cam.transform.position).y;

        Debug.Log("dist is y: " + dist);

        var northBorder = cam.ScreenToWorldPoint(new Vector3(0, 0, dist));

        transform.position = new Vector3(transform.position.x, transform.position.y, northBorder.z);

        //Vector3 newPosition = cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 1.0f));
        //Vector3 newPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        //newPosition.y = transform.position.y;
        //transform.position = new Vector3(newPosition.x, transform.position.y, 1.0f);
    }
}
