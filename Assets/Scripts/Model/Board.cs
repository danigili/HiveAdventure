using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Board
{
	// Peces
	private Dictionary<(int, int, int), Piece> placedPieces;
	private List<Piece> notPlacedPieces;
	private int turns;

	public Board()
	{
		placedPieces = new Dictionary<(int, int, int), Piece>();
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

	public Dictionary<(int, int, int), Piece> GetPlacedPieces()
	{
		 return placedPieces;
	}

	public List<Piece> GetNotPlacedPieces()
	{
		return notPlacedPieces;
	}

	public Board[] GetAllMovements(bool side)
	{
		//TODO
		return null;
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
		//TODO: FALTA COMPROBAR QUE SI ES EL QUART TORN, NOMÉS ES POT MOURE LA REINA

		//TODO
		List<Position> movements = new List<Position>();
		Position pos = GetPiecePosition(piece);
		if (pos == null)
		// La peça no està en joc, totes tenen el mateix comportament independentment del tipus d'insecte
		{
			
		}
		else
		// La peça està en joc, cada insecte te un comportament diferent
		{
			// Comprovem si es pot moure
			if (IsBlocked(piece))
				return movements;

			piece.type = BugType.Beetle;
			Position[] surroundings = GetSurroundings(pos);
			Position[] neighbors = GetNeighbors(pos);
			switch (piece.type)
			{
				case BugType.Queen:
					SlidePositions(surroundings, neighbors, ref movements);
					break;
				case BugType.Spider:
					
					break;
				case BugType.Beetle:
					SlidePositions(surroundings, neighbors, ref movements);
					foreach (Position p in neighbors)
						if (p != null && !placedPieces.ContainsKey((p.x, p.y, p.z+1)))
							movements.Add(new Position(p.x, p.y, p.z + 1));
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

					break;
			}
		}

		return movements;
	}


	public void SlidePositions(Position[] surroundings, Position[] neighbors, ref List<Position> movements)
	{
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

	public Position[] OuterPerimeter()
	{
		// TODO
		return null;
	}

	public Position[] Perimeter(Piece piece)
	{
		// TODO
		return null;
	}

	public bool MovePiece(Piece piece, (int, int, int) position)
	{
		//TODO FALTA COMPROBAR
		if (placedPieces.ContainsKey(position))
			return false;

		if (notPlacedPieces.Remove(piece))
		{
			placedPieces.Add(position, piece);
			return true;
		}

		// TODO FALTA SEGUIR

		return false;
	}

	public Position GetPiecePosition(Piece piece)
	{
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
			if (EqualityComparer<Piece>.Default.Equals(pair.Value, piece))
				return new Position(pair.Key.Item1, pair.Key.Item2);
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
			if (!placedPieces.ContainsKey((neighbors[i].x, neighbors[i].y, 0)))
				neighbors[i] = null;
		return neighbors;
	}

	public Position[] GetSurroundings(Position pos)
	{
		Position[] surroundings = new Position[6];
		if (pos.y % 2 == 0)
		{
			surroundings[0] = new Position(pos.x     , pos.y + 2);
			surroundings[1] = (new Position(pos.x + 1, pos.y + 1));
			surroundings[2] = (new Position(pos.x + 1, pos.y - 1));
			surroundings[3] = (new Position(pos.x    , pos.y - 2));
			surroundings[4] = (new Position(pos.x    , pos.y - 1));
			surroundings[5] = (new Position(pos.x    , pos.y + 1));
		}
		else
		{
			surroundings[0] = (new Position(pos.x    , pos.y + 2));
			surroundings[1] = (new Position(pos.x    , pos.y + 1));
			surroundings[2] = (new Position(pos.x    , pos.y - 1));
			surroundings[3] = (new Position(pos.x    , pos.y - 2));
			surroundings[4] = (new Position(pos.x - 1, pos.y - 1));
			surroundings[5] = (new Position(pos.x - 1, pos.y + 1));
		}
		return surroundings;
	}

	public Position GetSurrounding(Position pos, int direction)
	{
		if (pos.y % 2 == 0)
		{
			switch (direction)
			{
				case 0:	return new Position(pos.x    , pos.y + 2);
				case 1:	return new Position(pos.x + 1, pos.y + 1);
				case 2:	return new Position(pos.x + 1, pos.y - 1);
				case 3:	return new Position(pos.x    , pos.y - 2);
				case 4:	return new Position(pos.x    , pos.y - 1);
				case 5:	return new Position(pos.x    , pos.y + 1);
			}
		}
		else
		{
			switch (direction)
			{
				case 0:	return new Position(pos.x    , pos.y + 2);
				case 1:	return new Position(pos.x    , pos.y + 1);
				case 2:	return new Position(pos.x    , pos.y - 1);
				case 3:	return new Position(pos.x    , pos.y - 2);
				case 4:	return new Position(pos.x - 1, pos.y - 1);
				case 5: return new Position(pos.x - 1, pos.y + 1);
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
				if (placedPieces.ContainsKey((set[i].x, set[i].y, set[i].z + 1)))
					beetlesOnTop++;
			}
			i++;
		}
		if (set.Count + 1 + beetlesOnTop == placedPieces.Count) //TODO: FALTA TENIR EN COMPTE LA TERCERA DIMENSIÓ
			return false;
		return true;
	}
}
