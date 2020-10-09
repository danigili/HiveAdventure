using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceUI : MonoBehaviour
{
    public Piece piece;
    private Vector2 position;
    private bool remove = false;
    private Action<PieceUI> callback;
    private void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, position, Time.deltaTime*5);
        if (remove && Vector2.Distance(GetComponent<RectTransform>().anchoredPosition,position)<10)
            gameObject.SetActive(false);
    }

    public void Initialize(Piece piece, Vector2 pos, Action<PieceUI> callback)
    {
        this.piece = piece;
        this.callback = callback;
        this.position = pos;
        GetComponent<RectTransform>().anchoredPosition = position;
        remove = false;
        GetComponent<Button>().enabled = true;
    }

    public void Click()
    {
        callback(this);
    }

    public void SetPosition(Vector2 pos)
    {
        position = pos;
    }

    public void Remove()
    {
        SetPosition(new Vector2(position.x-200, position.y));
        GetComponent<Button>().enabled = false;
        remove = true;
    }

}
