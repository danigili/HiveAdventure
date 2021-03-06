﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    public class AIResult
    { 
        // Number of expanded nodes
        public int leaves;

        public int bestValue;

        // Piece and place of the best move
        public Move move;

        // Time
        public TimeSpan time;

        override
        public String ToString()
        {
            return time.ToString() + "\t-\t" + leaves.ToString() + "\t" + bestValue.ToString();
        }
    }

    public static AIResult FindBestMove(bool side, Board board, int depth)
    {
        DateTime inicio = DateTime.Now;
        
        AIResult result = new AIResult();
        int bestValue = side ? -100000 : 100000;
        List<Move> bestMoves = new List<Move>();
        int alpha = -100000;
        int beta = 100000;
        result.leaves = 0;
        board.side = side;
        for (int i = 0; i < board.Count(); i++)
        {
            List<Move> moves = board.GetAllMovements(i);
            for (int j = 0; j < moves.Count; j++)
            {
                Board newBoard = board.Clone();
                newBoard.MovePiece(moves[j].piece, moves[j].position);

                int value = EvaluateNode(newBoard, 1, !side, ref result.leaves, alpha, beta) + newBoard.EvaluateBoard()/10;
                if ((!side && value <= bestValue) || (side && value >= bestValue))
                {
                    if (bestValue != value)
                        bestMoves.Clear();
                    bestValue = value;
                    bestMoves.Add(moves[j]);
                }
            }
        }
        result.bestValue = bestValue;
        var random = new System.Random(DateTime.Now.Second+alpha+beta+result.leaves);
        result.move = bestMoves[random.Next() % bestMoves.Count];
        board.Initialize();
        result.time = DateTime.Now.Subtract(inicio);
        
        return result;
    }

    private static int EvaluateNode(Board board, int depth, bool side, ref int leaves, int alpha, int beta)
    {
        leaves++;
        
        if (depth > 2)
            return board.EvaluateBoard();

        Winner winner = board.CheckEndCondition();
        if (winner != Winner.None)
            return board.EvaluateBoard();

        board.side = side;
        bool blocked = true;
        for (int i = 0; i < board.Count(); i++)
        {
            List<Move> m = board.GetAllMovements(i);
            for (int j = 0; j < m.Count; j++)
            {
                blocked = false;

                if (beta <= alpha)
                    break;
                Board newBoard = board.Clone();
                newBoard.MovePiece(m[j].piece, m[j].position);
                if (side)
                    alpha = Math.Max(EvaluateNode(newBoard, depth + 1, !side, ref leaves, alpha, beta), alpha);
                else
                    beta = Math.Min(EvaluateNode(newBoard, depth + 1, !side, ref leaves, alpha, beta), beta);
            }
            if (beta <= alpha)
                break;
        }

        int value = side ? alpha : beta;
        if (blocked)
            value = 0;

        if (value > 1000)
            value -= depth*10;
        if (value < -1000)
            value += depth*10;

        return value;
    }


}
