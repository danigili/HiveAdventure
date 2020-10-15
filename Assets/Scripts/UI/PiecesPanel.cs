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
    public List<PieceUI> pieces;
    public List<GameObject> objects;

    public void Hola()
    {
        foreach (PieceUI pui in pieces)
            if (pui.gameObject.activeSelf)
                PlacePiece(pui.gameObject);
    }

    public void Initialize(Board model, Action<PieceUI> callback)
    {
        this.model = model;
        clickPieceCallback = callback;
        PlacePieces();
        PanelPosition();
    }

    private void PlacePieces()
    {
        pieces.Clear();
        // Disable current pieces
        foreach (GameObject obj in objects)
            obj.SetActive(false);

        Transform child = transform.GetChild(0);
        int j = 0;
        foreach (Piece piece in model.GetNotPlacedPieces())
        {
            if (side != piece.side) continue;

            GameObject instance;
            if (objects.Count > j && !objects[j].activeSelf)
            {
                instance = objects[j];
            }
            else
            {
                instance = Instantiate((GameObject)Resources.Load("Prefabs/PieceUI"), child);
                instance.gameObject.SetActive(false);
                objects.Add(instance);
            }
            j++;
            instance.GetComponent<PieceUI>().Initialize(piece, ClickPiece);

            PlacePiece(instance);
            pieces.Add(instance.GetComponent<PieceUI>());
        }
        
    }

    private void PlacePiece(GameObject instance)
    {
        Transform child = transform.GetChild(0);
        bool placed = false;
        bool firstPiece = true;
        Vector2 pos;
        if (instance.GetComponent<PieceUI>().piece.type == BugType.Ant)
            Debug.Log("");
        // Put the piece next to a piece of the same type
        for (int i = child.childCount - 1; i >= 0 && !placed; i--)
        {
            if (child.GetChild(i).gameObject.activeSelf)
            {
                firstPiece = false;
                if (child.GetChild(i).GetComponent<PieceUI>().piece.type == instance.GetComponent<PieceUI>().piece.type)
                {
                    pos.x = child.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x + xMargin;
                    pos.y = child.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y;
                    instance.GetComponent<RectTransform>().anchoredPosition = pos;
                    instance.GetComponent<PieceUI>().SetPosition(pos);
                    child.GetChild(i).GetComponent<Button>().enabled = false;
                    placed = true;
                    break;
                }
            }
        }
        if (placed) return;

        // If not, place under the last piece
        if (!firstPiece)
        {
            pos.x = StartPosition.anchoredPosition.x;
            pos.y = lastPos - yMargin;
            lastPos = pos.y;
            instance.GetComponent<PieceUI>().SetPosition(pos);
            instance.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        else
        // If there aren`t pieces, place in the start position
        {
            pos.x = StartPosition.anchoredPosition.x;
            pos.y = StartPosition.anchoredPosition.y;
            lastPos = pos.y;
            instance.GetComponent<PieceUI>().SetPosition(pos);
            instance.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    private float Hieght()
    {
        float hieght = -transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<RectTransform>().anchoredPosition.y + yPanelMargin;
        if (side)
            return -hieght;
        else
            return hieght;
    }

    private void PanelPosition()
    {
        Vector2 panelPos = transform.GetComponent<RectTransform>().anchoredPosition;
        panelPos.y = Hieght();
        transform.GetComponent<RectTransform>().anchoredPosition = panelPos;
    }

    public void ClickPiece(PieceUI piece)
    {
        clickPieceCallback(piece);
    }

    public void RemovePiece(PieceUI piece)
    {
        pieces.Remove(piece);
        piece.Remove();
        foreach (PieceUI pui in pieces)
        {
            if (pui.piece.type == piece.piece.type && pui.piece.side == piece.piece.side && pui.piece.number == piece.piece.number - 1)
                pui.GetComponent<Button>().enabled = true;
        }
    }
}
