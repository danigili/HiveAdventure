﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public enum Winner
{
	None,
	White,
	Black,
	Draw
}

[Serializable]
public class Board
{
	// Peces
	[SerializeField]
	private DualDictionary<(int, int, int), Piece> placedPieces;
	[SerializeField]
	private List<Piece> notPlacedPieces;
	[SerializeField]
	private int turns;
	
	[NonSerialized]
	private bool queen1Placed = false;
	[NonSerialized]
	private bool queen2Placed = false;

	public Board()
	{
		placedPieces = new DualDictionary<(int, int, int), Piece>();
		notPlacedPieces = new List<Piece>();
		Initialize();
	}

	public void Initialize()
	{
		FindQueens();
	}

	private void FindQueens()
	{
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Value.type == BugType.Queen)
			{
				if (!pair.Value.side)
					queen1Placed = true;
				else
					queen2Placed = true;
			}
		}
	}

	public Dictionary<(int, int, int), Piece> GetPlacedPieces()
	{
		return placedPieces;
	}

	public List<Piece> GetNotPlacedPieces()
	{
		return notPlacedPieces;
	}

	public Winner CheckEndCondition()
	{
		bool whiteLoses = false;
		bool blackLoses = false;
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Value.type == BugType.Queen)
			{
				if (!GetNeighbors(GetPiecePosition(pair.Value)).Contains(null))
				{
					if (pair.Value.side)
						blackLoses = true;
					else
						whiteLoses = true;
				}
			}
		}
		if (!blackLoses && !whiteLoses)
			return Winner.None;
		if (blackLoses && whiteLoses)
			return Winner.Draw;
		if (blackLoses && !whiteLoses)
			return Winner.White;
		if (!blackLoses && whiteLoses)
			return Winner.Black;
		return Winner.None;
	}

	public Piece GetPiece(Position pos)
	{
		if (placedPieces.ContainsKey((pos.x, pos.y, pos.z)))
			return placedPieces[(pos.x, pos.y, pos.z)];
		return null;
	}
	
	public Piece GetPiece(bool side, BugType type, int number)
	{
		Piece pieceSeek = new Piece(side, type, number);
		foreach (Piece p in notPlacedPieces)
			if (p.Equals(pieceSeek))
				return p;
		return null;
	}

	public List<Board> GetAllMovements(bool side)
	{
		List<Board> boards = new List<Board>();
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Value.side != side)
				continue;

			List<Position> movements = this.GetMovements(pair.Value);
			foreach (Position pos in movements)
			{
				Board newBoard = this.Clone();
				newBoard.MovePiece(pair.Value, pos);
				boards.Add(newBoard);
			}
		}

		List<Position> movements2 = new List<Position>();
		PlacePieceMovement(ref movements2, side);
		List<BugType> visitedPieces = new List<BugType>();
		foreach (Piece piece in notPlacedPieces)
		{
			if (piece.side != side)
				continue;

			if (visitedPieces.Contains(piece.type))
				break;

			visitedPieces.Add(piece.type);

			foreach (Position pos in movements2)
			{
				Board newBoard = this.Clone();
				newBoard.MovePiece(piece, pos);
				boards.Add(newBoard);
			}
		}
		return boards;
	}

	public bool IsBlocked(Piece piece)
	{
		// La peça no es pot moure si
		// Trenca la cohesió
		if (BreaksCohesion(piece))
			return true;

		// Si té una peça a sobre
		Position pos = GetPiecePosition(piece);
		if (placedPieces.ContainsKey((pos.x, pos.y, pos.z + 1)))
			return true;

		//TODO: FALTES MÉS CASOS
		return false;
	}

	public List<Position> GetMovements(Piece piece)
	{
		List<Position> movements = new List<Position>();
		Position pos = GetPiecePosition(piece);

		bool queenPlaced = piece.side ? queen2Placed : queen1Placed;

		if (!queenPlaced && piece.type != BugType.Queen && (turns >= 6))
			return movements;

		if (turns == 0)
		{
			movements.Add(new Position(0, 0, 0));
			return movements;
		}

		if (turns == 1)
		{
			movements.AddRange(GetSurroundings(GetPiecePosition(placedPieces.First().Value)));
			return movements;
		}

		if (pos != null && !queenPlaced)
			return movements;

		if (pos == null)
		// La peça no està en joc, totes tenen el mateix comportament independentment del tipus d'insecte
		{
			PlacePieceMovement(ref movements, piece.side);
		}
		else
		// La peça està en joc, cada insecte te un comportament diferent
		{
			// Comprovem si es pot moure
			if (IsBlocked(piece))
				return movements;

			Position[] surroundings = GetSurroundings(pos);
			Position[] neighbors = GetNeighbors(pos);
			switch (piece.type)
			{
				case BugType.Queen:
					SlidePositions(surroundings, neighbors, ref movements);
					break;
				case BugType.Spider:
					SpiderMovements(pos, pos, surroundings, neighbors, ref movements);
					break;
				case BugType.Beetle:
					BeetleMovements(pos, surroundings, neighbors, ref movements);
					break;
				case BugType.Grasshopper:
					for (int i = 0; i < surroundings.Length; i++)
					{
						Position nextPos = surroundings[i];
						while (placedPieces.ContainsKey((nextPos.x, nextPos.y, 0)))
							nextPos = GetSurrounding(nextPos, i);
						if (nextPos != surroundings[i])
							movements.Add(nextPos);
					}
					break;
				case BugType.Ant:
					Perimeter(pos, pos, surroundings, neighbors, ref movements);
					movements.Remove(pos);
					break;
			}
		}
		//Remove duplicates
		return movements.Distinct().ToList();
	}

	private void BeetleMovements(Position pos, Position[] surroundings, Position[] neighbors, ref List<Position> movements)
	{
		Position downPos = new Position(pos.x, pos.y, pos.z-1);

		for (int i = 0; i < neighbors.Length; i++)
		{
			int before = i == 0 ? 5 : i - 1;
			int after = i == 5 ? 0 : i + 1;

			// Comportament per sobre de les peces
			if (pos.z > 0 && (neighbors[before] == null || neighbors[after] == null))
			{
				if (placedPieces.ContainsKey((surroundings[i].x, surroundings[i].y, surroundings[i].z + 1)))
					continue;
				else if (placedPieces.ContainsKey((surroundings[i].x, surroundings[i].y, surroundings[i].z)))
					movements.Add(new Position(surroundings[i].x, surroundings[i].y, surroundings[i].z + 1));
				else if (placedPieces.ContainsKey((surroundings[i].x, surroundings[i].y, surroundings[i].z - 1)))
					movements.Add(surroundings[i]);
				else if (pos.z == 1 || placedPieces.ContainsKey((surroundings[i].x, surroundings[i].y, surroundings[i].z - 2)))
					movements.Add(new Position(surroundings[i].x, surroundings[i].y, surroundings[i].z - 1));
			}
			// Comportament a nivell de terra
			else 
			{
				if ((neighbors[before] == null) != (neighbors[after] == null) && neighbors[i] == null)
					movements.Add(surroundings[i]);
				if (neighbors[i] != null && !placedPieces.ContainsKey((surroundings[i].x, surroundings[i].y, surroundings[i].z + 1)))
					movements.Add(new Position(surroundings[i].x, surroundings[i].y, surroundings[i].z + 1));
			}
		}
	}

	private void SpiderMovements(Position origin, Position pos, Position[] surroundings, Position[] neighbors, ref List<Position> movements, int i=0, Position lastPos=null)
	{
		i++;
		if (i == 4)
		{
			movements.Add(pos);
			return;
		}
		List<Position> slidePositions = new List<Position>();
		SlidePositions(surroundings, neighbors, ref slidePositions, origin);
		foreach (Position dir in slidePositions)
		{
			if (dir.Equals(lastPos))
			{
				continue;
			}
			SpiderMovements(origin, dir, GetSurroundings(dir), GetNeighbors(dir), ref movements, i, pos);
		}
	}

	private void SlidePositions(Position[] surroundings, Position[] neighbors, ref List<Position> movements, Position ignorePos = null)
	{
		if (ignorePos != null)
			for (int i = 0; i < neighbors.Length; i++)
				if (ignorePos.Equals(neighbors[i]))
					neighbors[i] = null;

		for (int i = 0; i < neighbors.Length; i++)
		{
			if (neighbors[i] != null)
				continue;
			int before = i == 0 ? 5 : i - 1;
			int after = i == 5 ? 0 : i + 1;
			if ((neighbors[before] == null) != (neighbors[after] == null))
				movements.Add(surroundings[i]);
		}
	}

	public void PlacePieceMovement(ref List<Position> movements, bool side)
	{
		OuterPerimeter(ref movements);
		for (int i = movements.Count - 1; i >= 0; i--)
		{
			foreach (Position pos in GetNeighbors(movements[i]))
			{
				int j = 1;
				while (pos != null && placedPieces.ContainsKey((pos.x, pos.y, pos.z + j))) j++;
				if (pos != null && placedPieces[(pos.x, pos.y, pos.z + j - 1)].side != side)
				{
					movements.RemoveAt(i);
					break;
				}
			}
		}
	}

	private void OuterPerimeter(ref List<Position> movements)
	{
		KeyValuePair<(int, int, int), Piece> leftMostPiece = placedPieces.First();
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Key.Item2 < leftMostPiece.Key.Item2)
				leftMostPiece = pair;
		}
		Position pos = GetSurrounding(GetPiecePosition(leftMostPiece.Value), 3);
		Perimeter(pos, pos, GetSurroundings(GetPiecePosition(leftMostPiece.Value)), GetNeighbors(GetPiecePosition(leftMostPiece.Value)), ref movements);
	}

	private void Perimeter(Position origin, Position pos, Position[] surroundings, Position[] neighbors, ref List<Position> movements, int i = 0, Position lastPos = null)
	{
		i++;
		if (movements.Contains(pos))
		{
			return;
		}
		if (i != 0)
			movements.Add(pos);
		List<Position> slidePositions = new List<Position>();
		SlidePositions(surroundings, neighbors, ref slidePositions, origin);
		foreach (Position dir in slidePositions)
		{
			if (dir.Equals(lastPos))
			{
				continue;
			}
			Perimeter(origin, dir, GetSurroundings(dir), GetNeighbors(dir), ref movements, i, pos);
		}
	}

	public bool MovePiece(Piece piece, (int, int, int) position)
	{
		//TODO FALTA COMPROBAR
		if (placedPieces.ContainsKey(position))
			return false;

		if (notPlacedPieces.Remove(piece))
		{
			placedPieces.Add(position, piece);
		}
		else
		{
			Position oldPos = GetPiecePosition(piece);
			placedPieces.Remove((oldPos.x, oldPos.y, oldPos.z));
			placedPieces.Add(position, piece);
		}
		turns++;
		FindQueens();

		return true;
	}

	public bool MovePiece(Piece piece, Position position)
	{
		return MovePiece(piece, (position.x, position.y, position.z));
	}

	public Position GetPiecePosition(Piece piece)
	{
		if (placedPieces.ContainsValue(piece))
		{
			(int, int, int) p = placedPieces.getKey(piece);
			return new Position(p.Item1, p.Item2, p.Item3);
		}
		return null;
	}

	private Position[] GetNeighbors(Piece piece)
	{
		Position pos = GetPiecePosition(piece);
		if (pos == null) return null;
		return GetNeighbors(pos);
	}

	private Position[] GetNeighbors(Position pos)
	{
		Position[] neighbors = GetSurroundings(pos);
		for (int i = 0; i < neighbors.Length; i++)
			if (!placedPieces.ContainsKey((neighbors[i].x, neighbors[i].y, neighbors[i].z)))
				neighbors[i] = null;
		return neighbors;
	}

	public Position[] GetSurroundings(Position pos)
	{
		Position[] surroundings = new Position[6];
		if (pos.y % 2 == 0)
		{
			surroundings[0] = new Position(pos.x     , pos.y + 2, pos.z);
			surroundings[1] = (new Position(pos.x + 1, pos.y + 1, pos.z));
			surroundings[2] = (new Position(pos.x + 1, pos.y - 1, pos.z));
			surroundings[3] = (new Position(pos.x    , pos.y - 2, pos.z));
			surroundings[4] = (new Position(pos.x    , pos.y - 1, pos.z));
			surroundings[5] = (new Position(pos.x    , pos.y + 1, pos.z));
		}
		else
		{
			surroundings[0] = (new Position(pos.x    , pos.y + 2, pos.z));
			surroundings[1] = (new Position(pos.x    , pos.y + 1, pos.z));
			surroundings[2] = (new Position(pos.x    , pos.y - 1, pos.z));
			surroundings[3] = (new Position(pos.x    , pos.y - 2, pos.z));
			surroundings[4] = (new Position(pos.x - 1, pos.y - 1, pos.z));
			surroundings[5] = (new Position(pos.x - 1, pos.y + 1, pos.z));
		}
		return surroundings;
	}

	public Position GetSurrounding(Position pos, int direction)
	{
		if (pos.y % 2 == 0)
		{
			switch (direction)
			{
				case 0:	return new Position(pos.x    , pos.y + 2, pos.z);
				case 1:	return new Position(pos.x + 1, pos.y + 1, pos.z);
				case 2:	return new Position(pos.x + 1, pos.y - 1, pos.z);
				case 3:	return new Position(pos.x    , pos.y - 2, pos.z);
				case 4:	return new Position(pos.x    , pos.y - 1, pos.z);
				case 5:	return new Position(pos.x    , pos.y + 1, pos.z);
			}
		}
		else
		{
			switch (direction)
			{
				case 0:	return new Position(pos.x    , pos.y + 2, pos.z);
				case 1:	return new Position(pos.x    , pos.y + 1, pos.z);
				case 2:	return new Position(pos.x    , pos.y - 1, pos.z);
				case 3:	return new Position(pos.x    , pos.y - 2, pos.z);
				case 4:	return new Position(pos.x - 1, pos.y - 1, pos.z);
				case 5: return new Position(pos.x - 1, pos.y + 1, pos.z);
			}
		}
		return null;
	}

	public bool BreaksCohesion(Piece piece)
	{
		// Aquest mètode indica si la peça indicada trenca la cohesió del rusc, i per tant, no es pot moure
		// Ho fa de la següent manera: s'afegeix una peça en un set sense repeticions, per cada element del set
		// s'obtenen els seus veïns i s'afegeixen el set. Mai s'inclou la peça indicada al set.
		// Si la longitud final del set és igual al nombre de peces -1, significa que totes les peces son adjacents entre elles.
		List<Position> set = new List<Position>(); //TODO: FALTA OPTIMITZAR AMB ESTRUCTURA DE DADES PROPIA
		Position piecePos = GetPiecePosition(piece);
		if (piecePos.z != 0) // Els escarabats no poden trencar la cohesió
			return false;
		int i = 0;
		int beetlesOnTop = 0; // Els escarabats que estan per sobre d'altres peces no compten en el càlcul de la cohesió.
		if (placedPieces.First().Value != piece)
			set.Add(new Position(placedPieces.First().Key.Item1, placedPieces.First().Key.Item2));
		else
			set.Add(new Position(placedPieces.Last().Key.Item1, placedPieces.Last().Key.Item2));
		while (i < placedPieces.Count && i < set.Count)
		{
			if (!piecePos.Equals(set[i]))
			{
				Position[] neighbors = GetNeighbors(set[i]);
				foreach (Position p in neighbors)
					if (p!=null && !p.Equals(piecePos) && !set.Contains(p))
						set.Add(p);
				int j = 1;
				while (placedPieces.ContainsKey((set[i].x, set[i].y, set[i].z + j)))
				{ 
					beetlesOnTop++;
					j++;
				}
				
			}
			i++;
		}
		if (set.Count + 1 + beetlesOnTop == placedPieces.Count) 
			return false;
		return true;
	}

	public int EvaluateBoard()
	{
		int value = 0;
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Value.type == BugType.Queen)
			{
				int neighbors = 0;
				foreach (Position p in GetNeighbors(GetPiecePosition(pair.Value)))
					if (p!=null)
						neighbors++; 
				if (pair.Value.side)
				{
					if (neighbors == 6)
                        value -= 1000;
					else
						value -= neighbors;
				}
				else
				{
					if (neighbors == 6)
						value += 1000;
					else
						value += neighbors;
				}
			}
		}
		return value;
	}

	public Board Clone()
	{
		Board brother = new Board();
		brother.placedPieces = new DualDictionary<(int, int, int), Piece>(this.placedPieces);
		brother.notPlacedPieces = new List<Piece>(this.notPlacedPieces);
		brother.turns = this.turns;
		brother.Initialize();
		return brother;
	}
}
