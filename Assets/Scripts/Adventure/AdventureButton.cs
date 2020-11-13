﻿using System;
using UnityEngine;

public class AdventureButton : HexObject
{
    public int zone;
    public int level;
    public bool completed;
    public bool available;

    private float speed = 5f;
    private Action<AdventureButton> clickCallback;
    private TextMesh text;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
    }

    public void Initialize(int zone, int level, bool completed, bool available, Position position, Action<AdventureButton> clickCallback)
    {
        text = transform.GetChild(0).GetComponent<TextMesh>();
        this.zone = zone;
        this.level = level;
        this.completed = completed;
        this.available = available;
        this.SetHexPosition(position);
        this.clickCallback = clickCallback;
        SetTexture();
        text.text = zone + "-" + level;
        if (completed || available)
            text.color = Color.black;
        else
            text.color = Color.white;
        Vector3 pos = GetWorldPosition();
        var random = new System.Random(DateTime.Now.Second + level + zone);
        pos.y = random.Next()%100 + 20;
        transform.position = pos;
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
