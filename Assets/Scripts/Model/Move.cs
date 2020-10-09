using System.Collections;
using System.Collections.Generic;

public class Move
{
    public Piece piece;
    public Position position;

    public Move(Piece piece, Position position)
    {
        this.piece = piece;
        this.position = position;
    }
}
