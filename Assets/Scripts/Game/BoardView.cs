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

    public List<GameObject> markers = new List<GameObject>();

    void Start()
    {
        /*for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                GameObject instance = Instantiate(piecePrefab, this.transform);
                instance.GetComponent<PieceObject>().x = i;
                instance.GetComponent<PieceObject>().y = j;
            }
        }*/
    }

    void Update()
    {
        
    }

    public void Initialize(Board board)
    {
        this.model = board;
        Dictionary<(int, int, int), Piece>  placedPieces = model.GetPlacedPieces();
        List<Piece> notPlacedPieces = model.GetNotPlacedPieces();
        bool hola = true;
        foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().Initialize(pair.Value);
            instance.GetComponent<PieceObject>().x = pair.Key.Item1;
            instance.GetComponent<PieceObject>().y = pair.Key.Item2;
            instance.GetComponent<PieceObject>().z = pair.Key.Item3;
        }
        /*foreach (Piece p in notPlacedPieces)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().piece = p;
            instance.GetComponent<PieceObject>().x = -2;
            instance.GetComponent<PieceObject>().y = -2;
        }*/

    }

    public void PruobaMove()
    {

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

    public void ClickDown(PieceObject piece)
    {
        List<Position> positions = model.GetMovements(piece.piece);
        selectedPiece = piece;
        foreach (GameObject m in markers)
            m.SetActive(false);
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
        model.MovePiece(selectedPiece.piece, (marker.x, marker.y, marker.z));
        Position newPos = model.GetPiecePosition(selectedPiece.piece);
        selectedPiece.SetHexPosition(newPos.x, newPos.y, newPos.z);
        foreach (GameObject m in markers)
            m.SetActive(false);
    }
}
