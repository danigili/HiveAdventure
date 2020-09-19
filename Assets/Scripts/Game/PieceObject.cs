using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    public Piece piece;
    public int x;
    public int y;
    public static float distance = 1.8f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x =  distance * 1.73205f * x;
        pos.y = 0;
        pos.z = distance * y/2;
        if (y % 2 == 0)
            pos.x += distance * 1.73205f/2;
        transform.position = pos;
    }
}
