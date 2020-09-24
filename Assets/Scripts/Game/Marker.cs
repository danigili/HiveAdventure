using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : HexObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GetWorldPosition();
    }

    void OnMouseDown()
    {
        transform.GetComponentInParent<BoardView>().ClickDownMarker(this);
    }
}
