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

public class Piece
{
	public bool side;
	public BugType type;
	public int number;

	public Piece(bool side, BugType type, int number)
	{
		this.side = side;
		this.type = type;
		this.number = number;
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
}
