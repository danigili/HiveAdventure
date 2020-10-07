using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class AI
{
    public static Board FindBestMove(bool side, Board board, int depth)
    {
        int bestMove = 0;
        int bestValue = side ? -1000 : 1000;
        List<Board> movements = board.GetAllMovements(true);
        for (int i = 0; i < movements.Count; i++)
        {
            int value = EvaluateNode(movements[i], 0, side);
            if ((!side && value < bestValue) || (side && value > bestValue))
            {
                bestMove = i;
                bestValue = value;
            }
        }
        return (movements[bestMove]);
    }

    private static int EvaluateNode(Board board, int depth, bool side)
    {
        int value = side ? -1000 : 1000;

        if (depth > 0)
            return board.EvaluateBoard();

        foreach (Board b in board.GetAllMovements(side))
        {
            if (side)
                value = Math.Max(EvaluateNode(b, depth + 1, !side), value);
            else
                value = Math.Min(EvaluateNode(b, depth + 1, !side), value);
        }

        return value;
    }


}
