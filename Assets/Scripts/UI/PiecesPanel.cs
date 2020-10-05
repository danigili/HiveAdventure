using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecesPanel : MonoBehaviour
{
    private float xMargin = 20;
    private float yMargin = 105;
    private float yPanelMargin = 110;
    public RectTransform StartPosition;
    public bool side;
    private Board model;
    private float lastPos = 0;
    private Action<PieceUI> clickPieceCallback;
    public void Initialize(Board model, Action<PieceUI> callback)
    {
        this.model = model;
        clickPieceCallback = callback;
        PlacePieces();
    }

    private void PlacePieces()
    {
        foreach (Piece piece in model.GetNotPlacedPieces())
        {
            if (side != piece.side) continue;
            Transform child = transform.GetChild(0);
            bool placed = false;
            GameObject instance;
            
            Vector2 pos;
            // Put the piece next to a piece of the same type
            for (int i = child.childCount-1; i >= 0 && !placed; i--)
            {
                if (child.GetChild(i).GetComponent<PieceUI>().piece.type == piece.type)
                {
                    instance = Instantiate((GameObject)Resources.Load("Prefabs/PieceUI"), child);
                    pos.x = child.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x + xMargin;
                    pos.y = child.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y;
                    instance.GetComponent<PieceUI>().Initialize(piece, pos, ClickPiece);
                    child.GetChild(i).GetComponent<Button>().enabled = false;
                    placed = true;
                }
            }
            if (placed) continue;
            
            // If not, place under the last piece
            if (child.childCount > 0)
            {
                instance = Instantiate((GameObject)Resources.Load("Prefabs/PieceUI"), child);
                pos.x = StartPosition.anchoredPosition.x;
                pos.y = lastPos - yMargin;
                lastPos = pos.y;
                instance.GetComponent<PieceUI>().Initialize(piece, pos, ClickPiece);
                instance.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            // If there aren`t pieces, place in the start position
            {
                instance = Instantiate((GameObject)Resources.Load("Prefabs/PieceUI"), child);
                pos.x = StartPosition.anchoredPosition.x;
                pos.y = StartPosition.anchoredPosition.y;
                lastPos = pos.y;
                instance.GetComponent<PieceUI>().Initialize(piece, pos, ClickPiece);
                instance.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
        Vector2 panelPos = transform.GetComponent<RectTransform>().anchoredPosition;
        panelPos.y = Hieght();
        transform.GetComponent<RectTransform>().anchoredPosition = panelPos;
    }

    private float Hieght()
    {
        float hieght = -transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<RectTransform>().anchoredPosition.y + yPanelMargin;
        if (side)
            return -hieght;
        else
            return hieght;
    }

    public void ClickPiece(PieceUI piece)
    {
        clickPieceCallback(piece);
    }
}
