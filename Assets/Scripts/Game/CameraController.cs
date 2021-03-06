﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 position;
    public float size;
    private float angle;
    private float speed = 10;
    public float rotationSpeed = 2;
    private Camera camera;
    private float height = 6.87f;
    private float xCenter, yCenter;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        position = new Vector3(0, 6.87f, -4f);
        transform.position = position;
        size = 2.65f;
        angle = 60;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        if (time > 0.05f) time = 0.05f;
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, size, speed * time);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(angle, 0, 0), rotationSpeed * time);
        if (Mathf.Abs(transform.eulerAngles.x - angle)<0.2f)
            transform.position = Vector3.Lerp(transform.position, position, speed * time);
        else
            transform.position = new Vector3(xCenter, height, yCenter - Mathf.Tan(Mathf.PI / 180 * (90 - transform.eulerAngles.x)) * height) ;
    }

    public void SetCenter(float x, float y, bool smooth = true)
    {
        xCenter = x;
        yCenter = y;
        position.x = x;
        position.y = height;
        position.z = y - Mathf.Tan(Mathf.PI /180 * (90-angle)) * height ;
        if (!smooth)
            transform.position = new Vector3(xCenter, height, yCenter - Mathf.Tan(Mathf.PI / 180 * (90 - transform.eulerAngles.x)) * height);
    }

    public void SetSize(float size, bool smooth = true)
    {
        this.size = size;
        if (!smooth)
            camera.orthographicSize = size;
    }

    // 60 or 90 degrees
    public void SetAngle(float angle)
    {
        this.angle = angle;
    }
}
