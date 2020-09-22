using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public GameObject piecePrefab;
    private Board model;
    public List<PieceObject> pieces;

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
        foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().piece = pair.Value;
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
        //Debug.Log(transform.GetComponentsInChildren<PieceObject>().Length);
        //model.MovePiece(transform.GetComponentsInChildren<PieceObject>()[8].piece, (0, -2));
        //UpdatePositions();

        Position[] positions = model.GetSurroundings(new Position(-1, -1));
        foreach (Position pos in positions)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().x = pos.x;
            instance.GetComponent<PieceObject>().y = pos.y;
        }
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

    public void ClickDown(Piece piece)
    {
        Debug.Log(model.BreaksCohesion(piece));
        List<Position> positions = model.GetMovements(piece);

        foreach (Position pos in positions)
        {
            GameObject instance = Instantiate(piecePrefab, transform); //TODO: PONER EN UNA SOLA LINEA
            instance.GetComponent<PieceObject>().x = pos.x;
            instance.GetComponent<PieceObject>().y = pos.y;
            instance.GetComponent<PieceObject>().z = pos.z;
        }
    }


}
