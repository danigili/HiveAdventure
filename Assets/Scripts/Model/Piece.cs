using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugType 
{
	Queen,
	Spider,
	Beetle,
	Grasshopper,
	Ant
}

[System.Serializable]
public class Piece
{
	public bool side;
	public BugType type;
	public int number;
	[NonSerialized]
	public Position position;

	public Piece(bool side, BugType type, int number)
	{
		this.side = side;
		this.type = type;
		this.number = number;
		this.position = null;
	}

	public void SetPosition((int, int, int) position)
	{
		if (this.position == null)
			this.position = new Position(position);
		else
			this.position.SetPosition(position);
	}

	public string GetBugTypeName()
	{
		switch (type)
		{
			case BugType.Queen:       return "queen";
			case BugType.Spider:      return "spider";
			case BugType.Beetle:      return "beetle";
			case BugType.Grasshopper: return "grasshopper";
			case BugType.Ant:         return "ant";
		}
		return "";
	}
	
	public override bool Equals(object obj)
	{
		if (obj == null)
			return false;
		if (obj is Piece)
			return this.side == ((Piece)obj).side && this.type == ((Piece)obj).type && this.number == ((Piece)obj).number;
		return false;
	}

	public override int GetHashCode()
	{
		return ("" + (side ? "1" : "2") + "," + this.GetBugTypeName() + "," + number).GetHashCode();
	}
}
