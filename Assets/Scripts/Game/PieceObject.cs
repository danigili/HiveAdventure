using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    public Piece piece;
    public int x;
    public int y;
    public int z;
    public static float distance = 1.8f;
    public static float height = 0.8f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x =  distance * 1.73205f * x;
        pos.y = height * z;
        pos.z = distance * y/2;
        if (y % 2 == 0)
            pos.x += distance * 1.73205f/2;
        transform.position = pos;
    }

    void OnMouseDown()
    {
        transform.GetComponentInParent<BoardView>().ClickDown(piece);
    }
}
