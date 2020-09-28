using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System;

[Serializable]
public class Board
{
	// Peces
	[SerializeField]
	private Dictionary<(int, int, int), Piece> placedPieces;
	[SerializeField]
	private List<Piece> notPlacedPieces;
	[SerializeField]
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
		notPlacedPieces.Add(new Piece(false, BugType.Beetle, 3));
		notPlacedPieces.Add(new Piece(false, BugType.Beetle, 4));
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

	public Piece GetPiece(Position pos)
	{
		if (placedPieces.ContainsKey((pos.x, pos.y, pos.z)))
			return placedPieces[(pos.x, pos.y, pos.z)];
		return null;
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

			Position[] surroundings = GetSurroundings(pos);
			Position[] neighbors = GetNeighbors(pos);
			switch (piece.type)
			{
				case BugType.Queen:
					SlidePositions(surroundings, neighbors, ref movements);
					break;
				case BugType.Spider:
					// TODO
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
					// TODO
					break;
			}
		}

		return movements;
	}

	public void BeetleMovements(Position pos, Position[] surroundings, Position[] neighbors, ref List<Position> movements)
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

	public void SpiderMovements(Position pos, Position[] surroundings, Position[] neighbors, ref List<Position> movements)
	{ 
		
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
		KeyValuePair<(int, int, int), Piece> leftMostPiece = placedPieces.First();
		foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
		{
			if (pair.Key.Item2 < leftMostPiece.Key.Item2)
				leftMostPiece = pair;
		}
		return Perimeter(GetSurrounding(new Position(leftMostPiece.Key.Item1, leftMostPiece.Key.Item2, leftMostPiece.Key.Item3), 3));
	}

	public Position[] Perimeter(Position start)
	{
		
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
			piece.SetPosition(position);
			return true;
		}
		else
		{
			Position oldPos = GetPiecePosition(piece);
			placedPieces.Remove((oldPos.x, oldPos.y, oldPos.z));
			placedPieces.Add(position, piece);
			piece.SetPosition(position);
		}

		// TODO FALTA SEGUIR

		return false;
	}

	public Position GetPiecePosition(Piece piece)
	{
		return piece.position;
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
		if (set.Count + 1 + beetlesOnTop == placedPieces.Count) //TODO: FALTA TENIR EN COMPTE LA TERCERA DIMENSIÓ
			return false;
		return true;
	}
}
