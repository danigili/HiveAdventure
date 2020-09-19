using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{

    private Board model = new Board();
    public List<PieceObject> pieces;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlacePieces()
    {
        Dictionary<(int, int), Piece>  placedPieces = model.GetPlacedPieces();

    }
}
