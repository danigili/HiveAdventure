using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public BoardView boardView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click1()
    {
        Debug.Log("HOLA");
        Board board = new Board();
        List<Piece> notPlaced = board.GetNotPlacedPieces();
        Piece p = notPlaced[0];
        board.MovePiece(notPlaced[0], (-1,-2));//
        board.MovePiece(notPlaced[0], (0, -2));//
        board.MovePiece(notPlaced[0], (-1, -1));//
        board.MovePiece(notPlaced[0], (-1, 0));//
        board.MovePiece(notPlaced[0], (0, -1));//
        board.MovePiece(notPlaced[0], (1, 0));
        board.MovePiece(notPlaced[0], (1, 1));
        board.MovePiece(notPlaced[0], (0, 0));
        boardView.GetComponent<BoardView>().Initialize(board);
        for (int i = 0; i < 100000; i++)
        {
            board.BreaksCohesion(p);
        }
        Debug.Log("Breaks");
    }
}
