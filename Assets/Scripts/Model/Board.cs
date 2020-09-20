using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

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
		//TODO
		return false;
	}

	public List<(int, int)> GetMovements(Piece piece)
	{
		//TODO
		return null;
	}

	public bool MovePiece(Piece piece, (int, int) position)
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
		foreach (KeyValuePair<(int, int), Piece> pair in placedPieces)
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
			if (!placedPieces.ContainsKey((neighbors[i].x, neighbors[i].y)))
				neighbors[i] = null;
		return neighbors;
	}

	public Position[] GetSurroundings(Position pos)
	{
		Position[] surroundings = new Position[6];
		if (pos.y % 2 == 0)
		{
			surroundings[0] = new Position(pos.x    , pos.y + 2);
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

	public bool BreaksCohesion(Piece piece)
	{
		// Aquest mètode indica si la peça indicada trenca la cohesió del rusc, i per tant, no es pot moure
		// Ho fa de la següent manera: s'afegeix una peça en un set sense repeticions, per cada element del set
		// s'obtenen els seus veïns i s'afegeixen el set. Mai s'inclou la peça indicada al set.
		// Si la longitud final del set és igual al nombre de peces -1, significa que totes les peces son adjacents entre elles.
		List<Position> set = new List<Position>(); //TODO: FALTA OPTIMITZAR AMB ESTRUCTURA DE DADES PROPIA
		Position piecePos = GetPiecePosition(piece);
		int i = 0;
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
			}
			i++;
		}
		if (set.Count + 1 == placedPieces.Count)
			return false;
		return true;
	}
}
