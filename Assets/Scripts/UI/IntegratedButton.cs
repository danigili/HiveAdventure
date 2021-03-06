﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Vector3 pos = transform.position;
        pos.y = 1f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed*2);
    }

    private void OnMouseDown()
    {
        if (IsPointerOverUIObject() || EventSystem.current.IsPointerOverGameObject())
            return;
        StartCoroutine(GameObject.FindObjectOfType<GameMain>().IntegratedButtonClick(option));        
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
