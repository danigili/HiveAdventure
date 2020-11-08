using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureButton : HexObject
{
    public int zone;
    public int level;
    public bool completed;
    public bool available;

    private float speed = 5f;
    private Action<AdventureButton> clickCallback;
    private Text text;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
    }

    public void Initialize(int zone, int level, bool completed, bool available, Action<AdventureButton> clickCallback)
    {
        this.zone = zone;
        this.level = level;
        this.completed = completed;
        this.available = available;
        this.clickCallback = clickCallback;
        SetTexture();
        text.text = zone + "-" + level;
        if (completed || available)
            text.color = Color.black;
        else
            text.color = Color.white;
    }

    private void SetTexture()
    {
        Texture2D texture;
        if (available || completed)
            texture = Resources.Load<Texture2D>("Textures/white");
        else
            texture = Resources.Load<Texture2D>("Textures/black");
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    private void OnMouseOver()
    {
        if (available)
        {
            Vector3 pos = transform.position;
            pos.y = 1f;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed * 2);
        }
    }

    private void OnMouseDown()
    {
        if (available)
            clickCallback(this);
    }
}
