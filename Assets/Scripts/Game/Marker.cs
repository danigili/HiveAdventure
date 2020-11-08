using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Marker : HexObject
{
    private Action<Marker> clickCallback;

    public void Initialize(Action<Marker> clickCallback)
    {
        this.clickCallback = clickCallback;
    }

    void OnMouseDown()
    {
        clickCallback(this);
    }
}
