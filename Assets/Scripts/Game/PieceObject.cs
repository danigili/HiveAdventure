using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceObject : HexObject
{
    public Piece piece;


    void Start()
    {
        
    }

    void Update()
    {
        transform.position = GetWorldPosition();
    }

    void OnMouseDown()
    {
        transform.GetComponentInParent<BoardView>().ClickDown(this);
    }


    public void Initialize(Piece piece)
    {
        this.piece = piece;
        SetTexture();        
    }

    private void SetTexture()
    { 
        Texture2D texture = Resources.Load<Texture2D>("Textures/" + piece.GetBugTypeName() + "_" + (piece.side ? "black" : "white") + "_bw");
        if (texture == null)
            texture = Resources.Load<Texture2D>("Textures/green");
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
