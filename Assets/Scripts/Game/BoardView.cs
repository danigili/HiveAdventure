using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using FullSerializer;

public class BoardView : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject markerPrefab;
    private Board model;
    public List<PieceObject> pieces;
    public PieceObject selectedPiece;
    public PieceUI selectedUIPiece;
    public PiecesPanel panel1;
    public PiecesPanel panel2;

    public List<GameObject> markers = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Initialize(Board board)
    {
        this.model = board;
        Dictionary<(int, int, int), Piece>  placedPieces = model.GetPlacedPieces();
        List<Piece> notPlacedPieces = model.GetNotPlacedPieces();

        foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().Initialize(pair.Value);
            instance.GetComponent<PieceObject>().x = pair.Key.Item1;
            instance.GetComponent<PieceObject>().y = pair.Key.Item2;
            instance.GetComponent<PieceObject>().z = pair.Key.Item3;
        }

        panel1.Initialize(model, ClickPanelPiece);
        panel2.Initialize(model, ClickPanelPiece);
    }



    public void PruobaMove()
    {
        DateTime inicio = DateTime.Now;
        for (int i = 0; i < 1000; i++)
            model.GetAllMovements(true);
        Debug.Log((DateTime.Now - inicio).Minutes + ":" + (DateTime.Now - inicio).Seconds + "." + (DateTime.Now - inicio).Milliseconds);
        Debug.Log(model.GetAllMovements(false).Count);

        
        
    }

    public void Prueba2()
    {
        Debug.Log(BoardSerialization.ToJson(model));
        DateTime inicio = DateTime.Now;
        for (int i = 0; i < 100000; i++)
            model.BreaksCohesion(model.GetPlacedPieces().First().Value);
        Debug.Log((DateTime.Now - inicio).Minutes + ":" + (DateTime.Now - inicio).Seconds + "." + (DateTime.Now - inicio).Milliseconds); 
    }

    public void AIButton()
    {
        DateTime inicio = DateTime.Now;
        int leaves = 0;
        int bestValue = 0;
        Debug.Log(BoardSerialization.ToJson(AI.FindBestMove(true, model, 0, out leaves, out bestValue)));
        Debug.Log((DateTime.Now - inicio).Minutes + ":" + (DateTime.Now - inicio).Seconds + "." + (DateTime.Now - inicio).Milliseconds);
        Debug.Log("Leaves: " + leaves + ", Value: " + bestValue);
    }

    public void Evaluate()
    {
        Debug.Log(model.EvaluateBoard());
    }

    public void UpdatePositions()
    {
        PieceObject[] pieces = transform.GetComponentsInChildren<PieceObject>();
        foreach (PieceObject piece in pieces)
        {
            Dictionary<(int, int, int), Piece> placedPieces = model.GetPlacedPieces();
            foreach(KeyValuePair < (int, int, int), Piece > pair in placedPieces)    
            {
                if (pair.Value.Equals(piece.piece))
                {
                    piece.x = pair.Key.Item1;
                    piece.y = pair.Key.Item2;
                    piece.z = pair.Key.Item3;
                }
            }
        }

    }

    public void ClickPanelPiece(PieceUI piece)
    {
        List<Position> positions = model.GetMovements(piece.piece);

        foreach (GameObject m in markers)
            m.SetActive(false);
        if (piece.Equals(selectedUIPiece))
        {
            selectedUIPiece = null;
            return;
        }
        selectedPiece = null;
        selectedUIPiece = piece;
        for (int i = 0; i < positions.Count; i++)
        {
            if (markers.Count > i && !markers[i].activeSelf)
            {
                markers[i].GetComponent<Marker>().SetHexPosition(positions[i].x, positions[i].y, positions[i].z);
                markers[i].SetActive(true);
            }
            else
            {
                GameObject instance = Instantiate(markerPrefab, transform); //TODO: PONER EN UNA SOLA LINEA
                instance.GetComponent<Marker>().SetHexPosition(positions[i].x, positions[i].y, positions[i].z);
                markers.Add(instance);
            }
        }
    }

    public void ClickDown(PieceObject piece)
    {
        List<Position> positions = model.GetMovements(piece.piece);
        
        foreach (GameObject m in markers)
            m.SetActive(false);
        if (piece.Equals(selectedPiece))
        {
            selectedPiece = null;
            return;
        }
        selectedPiece = piece;
        selectedUIPiece = null;
        for (int i = 0; i <positions.Count; i++)
        {
            if (markers.Count > i && !markers[i].activeSelf)
            {
                markers[i].GetComponent<Marker>().SetHexPosition(positions[i].x, positions[i].y, positions[i].z);
                markers[i].SetActive(true);
            }
            else
            {
                GameObject instance = Instantiate(markerPrefab, transform); //TODO: PONER EN UNA SOLA LINEA
                instance.GetComponent<Marker>().SetHexPosition(positions[i].x, positions[i].y, positions[i].z);
                markers.Add(instance);
            }
        }
    }

    public void ClickDownMarker(Marker marker)
    {
        if (selectedPiece != null)
        {
            model.MovePiece(selectedPiece.piece, (marker.x, marker.y, marker.z));
            Position newPos = model.GetPiecePosition(selectedPiece.piece);
            selectedPiece.SetHexPosition(newPos.x, newPos.y, newPos.z);
            selectedPiece = null;
        }
        else if (selectedUIPiece != null)
        {
            selectedUIPiece.Remove();
            model.MovePiece(selectedUIPiece.piece, (marker.x, marker.y, marker.z));
            Position newPos = model.GetPiecePosition(selectedUIPiece.piece);
            GameObject instance = Instantiate(piecePrefab, transform);
            instance.GetComponent<PieceObject>().Initialize(selectedUIPiece.piece);
            instance.GetComponent<PieceObject>().x = model.GetPiecePosition(selectedUIPiece.piece).x;
            instance.GetComponent<PieceObject>().y = model.GetPiecePosition(selectedUIPiece.piece).y;
            instance.GetComponent<PieceObject>().z = model.GetPiecePosition(selectedUIPiece.piece).z;
            selectedUIPiece = null;
        }
        foreach (GameObject m in markers)
            m.SetActive(false);
    }
}
