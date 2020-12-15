using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceObject : HexObject
{
    public Piece piece;

    private Action<PieceObject> clickCallback;

    void Start()
    {
        
    }

    void OnMouseDown()
    {
        if (IsPointerOverUIObject() || EventSystem.current.IsPointerOverGameObject())
            return;

        clickCallback(this);
    }


    public void Initialize(Piece piece, Action<PieceObject> clickCallback)
    {
        this.piece = piece;
        this.clickCallback = clickCallback;
        SetTexture();        
    }

    private void SetTexture()
    { 
        Texture2D texture = Resources.Load<Texture2D>("Textures/" + piece.GetBugTypeName() + "_" + (piece.side ? "black" : "white") + "_bw");
        if (texture == null)
            texture = Resources.Load<Texture2D>("Textures/green");
        GetComponent<Renderer>().material.mainTexture = texture;
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
