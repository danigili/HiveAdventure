using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a piece that behaves like a button in the interated 3D interface.
public class IntegratedButton : MonoBehaviour
{
    public GameMode option;

    private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
    }

    private void OnMouseOver()
    {
        Vector3 pos = transform.position;
        pos.y = 1f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed*2);
    }

    private void OnMouseDown()
    {
        StartCoroutine(GameObject.FindObjectOfType<GameMain>().IntegratedButtonClick(option));        
    }
}
