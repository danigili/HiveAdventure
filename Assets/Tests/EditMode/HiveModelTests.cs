using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using UnityEngine.UIElements;

namespace Tests
{
    public class HiveModelTests
    {
        private string resourcesPath = "./Assets/Tests/Resources/";
        // A Test behaves as an ordinary method
        [Test]
        public void LoadBoard()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test1.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Assert.IsTrue(board.GetPlacedPieces().ContainsKey((0, 0, 0)));
            Assert.AreEqual(board.GetPlacedPieces()[(0, 0, 0)].type, BugType.Ant);
        }

        // Test cohesion
        [Test]
        public void Cohesion()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position( 0,  2, 0))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position( 1,  1, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position( 0,  0, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position( 0, -1, 0))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position(-1,  0, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position( 0, -3, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position( 0, -3, 1))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position( 0, -3, 2))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position( 0, -5, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position(-1, -4, 0))));
            Assert.IsTrue(board.IsBlocked(board.GetPiece( new Position(-1, -3, 0))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position(-1, -3, 1))));
            Assert.IsFalse(board.IsBlocked(board.GetPiece(new Position(-2, -2, 0))));
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
            StreamReader reader = new StreamReader(resourcesPath + "test1.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece queen = board.GetPiece(new Position(-1, 0, 0));
            List<Position> movements = board.GetMovements(queen);
            Assert.IsFalse(movements.Contains(new Position(-1, -2, 0)));
            Assert.IsTrue( movements.Contains(new Position(-1,  1, 0)));
            Assert.IsTrue( movements.Contains(new Position( 0,  1, 0)));
            Assert.IsFalse(movements.Contains(new Position(-1,  2, 0)));
        }

        // Test the Spider Movement
        [Test]
        public void Spider()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test2.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece spider = board.GetPiece(new Position(0, 2, 0));
            List<Position> movements = board.GetMovements(spider);

            Assert.AreEqual(4, movements.Count);
            Assert.IsTrue(movements.Contains(new Position(-1, -1, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1, -2, 0)));
            Assert.IsTrue(movements.Contains(new Position( 1,  0, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1,  4, 0)));
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
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece ant = board.GetPiece(new Position(1, 1, 0));
            Assert.AreEqual(18, board.GetMovements(ant).Count);
        }

        // Test the Grasshopper Movement
        [Test]
        public void Grasshopper()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = BoardSerialization.FromJson(reader.ReadToEnd());
            Piece grasshopper = board.GetPiece(new Position(0, -5, 0));
            Assert.AreEqual(2, board.GetMovements(grasshopper).Count);
            board.MovePiece(grasshopper, (0, 1, 0));
            List<Position> movements = board.GetMovements(grasshopper);
            Assert.AreEqual(4, movements.Count);
            Assert.IsTrue(movements.Contains(new Position( 1,  3, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1, -1, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1, -1, 0)));
            Assert.IsTrue(movements.Contains(new Position( 0, -5, 0)));
        }

    }
}