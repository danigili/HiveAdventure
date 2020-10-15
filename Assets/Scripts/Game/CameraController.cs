using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 position;
    public float size;
    private float angle;
    public float speed = 10;
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
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, size, speed * Time.deltaTime);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(angle, 0, 0), rotationSpeed * Time.deltaTime);
        if (Mathf.Abs(transform.eulerAngles.x - angle)<0.2f)
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        else
            transform.position = new Vector3(xCenter, height, yCenter - Mathf.Tan(Mathf.PI / 180 * (90 - transform.eulerAngles.x)) * height) ;
    }

    public void SetCenter(float x, float y)
    {
        xCenter = x;
        yCenter = y;
        position.x = x;
        position.y = height;
        position.z = y - Mathf.Tan(Mathf.PI /180 * (90-angle)) * height ;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    // 60 or 90 degrees
    public void SetAngle(float angle)
    {
        this.angle = angle;
    }
}
