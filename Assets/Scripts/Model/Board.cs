using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Board
{
	// Peces
	private Dictionary<(int, int), Piece> placedPieces;
	private List<Piece> notPlacedPieces;
	private int turns;

	public Board()
	{
		placedPieces = new Dictionary<(int, int), Piece>();
		notPlacedPieces = new List<Piece>();
		Initialize();
	}

	// Afegeix les peces del joc
	public void Initialize()
	{
		//TODO: FALTA OPTIMITZAR
		notPlacedPieces.Clear();
		placedPieces.Clear();
		turns = 0;
		notPlacedPieces.Add(new Piece(false, BugType.Queen, 0));
		notPlacedPieces.Add(new Piece(false, BugType.Spider, 0));
		notPlacedPieces.Add(new Piece(false, BugType.Spider, 1));
		notPlacedPieces.Add(new Piece(false, BugType.Beetle, 0));
		notPlacedPieces.Add(new Piece(false, BugType.Beetle, 1));
		notPlacedPieces.Add(new Piece(false, BugType.Grasshopper, 0));
		notPlacedPieces.Add(new Piece(false, BugType.Grasshopper, 1));
		notPlacedPieces.Add(new Piece(false, BugType.Grasshopper, 2));
		notPlacedPieces.Add(new Piece(false, BugType.Ant, 0));
		notPlacedPieces.Add(new Piece(false, BugType.Ant, 1));
		notPlacedPieces.Add(new Piece(false, BugType.Ant, 2));
		notPlacedPieces.Add(new Piece(true, BugType.Queen, 0));
		notPlacedPieces.Add(new Piece(true, BugType.Spider, 0));
		notPlacedPieces.Add(new Piece(true, BugType.Spider, 1));
		notPlacedPieces.Add(new Piece(true, BugType.Beetle, 0));
		notPlacedPieces.Add(new Piece(true, BugType.Beetle, 1));
		notPlacedPieces.Add(new Piece(true, BugType.Grasshopper, 0));
		notPlacedPieces.Add(new Piece(true, BugType.Grasshopper, 1));
		notPlacedPieces.Add(new Piece(true, BugType.Grasshopper, 2));
		notPlacedPieces.Add(new Piece(true, BugType.Ant, 0));
		notPlacedPieces.Add(new Piece(true, BugType.Ant, 1));
		notPlacedPieces.Add(new Piece(true, BugType.Ant, 2));
	}

	public Dictionary<(int, int), Piece> GetPlacedPieces()
	{
		 return placedPieces;
	}

	public Board[] GetAllMovements(bool side)
	{
		//TODO
		return null;
	}

	public bool IsBlocked(Piece piece)
	{
		//TODO
		return false;
	}

	public List<(int, int)> GetMovements(Piece piece)
	{
		//TODO
		return null;
	}

	public void MovePiece(Piece piece, (int, int) position)
	{
		//TODO 
	}

	private List<Position> GetNeighbors(Piece piece)
	{
		foreach (KeyValuePair<(int, int), Piece> pair in placedPieces)
		{
			if (EqualityComparer<Piece>.Default.Equals(pair.Value, piece))
			{
				Position pos = new Position(pair.Key.Item1, pair.Key.Item2);
				List<Position> neighbors = GetSurroundings(pos);
				for (int i = 0; i < neighbors.Count; i++)
				{
					if (!placedPieces.ContainsKey((neighbors[i].x, neighbors[i].y)))
						neighbors.Insert(i, null);
				}
				return neighbors;
			}
		}

		return null;
	}

	private List<Position> GetSurroundings(Position pos)
	{
		List<Position> surroundings = new List<Position>();

		if (pos.y % 2 == 0)
		{
			surroundings.Add(new Position(pos.x    , pos.y + 2));
			surroundings.Add(new Position(pos.x + 1, pos.y + 1));
			surroundings.Add(new Position(pos.x + 1, pos.y - 1));
			surroundings.Add(new Position(pos.x    , pos.y - 2));
			surroundings.Add(new Position(pos.x    , pos.y - 1));
			surroundings.Add(new Position(pos.x    , pos.y + 1));
		}
		else
		{
			surroundings.Add(new Position(pos.x    , pos.y + 2));
			surroundings.Add(new Position(pos.x    , pos.y + 1));
			surroundings.Add(new Position(pos.x    , pos.y - 1));
			surroundings.Add(new Position(pos.x    , pos.y - 2));
			surroundings.Add(new Position(pos.x - 1, pos.y - 1));
			surroundings.Add(new Position(pos.x - 1, pos.y + 1));
		}
		return null;
	}

	private bool BreaksCohesion(Piece piece)
	{
		
		return false;
	}
}
