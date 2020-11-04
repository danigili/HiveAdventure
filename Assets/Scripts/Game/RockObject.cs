using System;
using UnityEngine;

public class RockObject : HexObject
{
    private Action<RockObject> clickCallback;

    public void Initialize(Action<RockObject> clickCallback)
    {
        this.clickCallback = clickCallback;
    }

    void OnMouseDown()
    {
        clickCallback(this);
    }
}
