using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using System.Net.Http.Headers;

namespace Tests
{
    public class HiveModelTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void LoadBoard()
        {
            StreamReader reader = new StreamReader("./Assets/Tests/Resources/test1.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Assert.IsTrue(board.GetPlacedPieces().ContainsKey((0, 0, 0)));
            Assert.AreEqual(board.GetPlacedPieces()[(0, 0, 0)].type, BugType.Ant);
        }

        // Test cohesion
        [Test]
        public void Cohesion()
        {
            // TODO
        }

        // Test the Win Condition
        [Test]
        public void WinCondition()
        {
            // TODO
        }

        // Test the Queen Movement
        [Test]
        public void Queen()
        {
            StreamReader reader = new StreamReader("./Assets/Tests/Resources/test1.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece queen = board.GetPiece(new Position(-1, 0, 0));
            List<Position> movements = board.GetMovements(queen);
            Assert.IsFalse(movements.Contains(new Position(-1, -2, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1, 1, 0)));
            Assert.IsTrue(movements.Contains(new Position(0, 1, 0)));
            Assert.IsFalse(movements.Contains(new Position(-1, 2, 0)));
        }

        // Test the Spider Movement
        [Test]
        public void Spider()
        {
            StreamReader reader = new StreamReader("./Assets/Tests/Resources/test1.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece spider = board.GetPiece(new Position(-1, 0, 0));
            List<Position> movements = board.GetMovements(spider);
            // TODO
            Assert.IsFalse(movements.Contains(new Position(-1, -2, 0)));
        }

        // Test the Beetle Movement
        [Test]
        public void Beetle()
        {
            // TODO
        }

        // Test the Ant Movement
        [Test]
        public void Ant()
        {
            // TODO
        }

        // Test the Grasshopper Movement
        [Test]
        public void Grasshopper()
        {
            
            // TODO
        }

    }
}