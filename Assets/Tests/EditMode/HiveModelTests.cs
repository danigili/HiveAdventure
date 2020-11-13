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
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            Assert.IsTrue(board.GetPlacedPieces().ContainsKey((0, 0, 0)));
            Assert.AreEqual(board.GetPlacedPieces()[(0, 0, 0)].type, BugType.Ant);
        }

        // Test cohesion
        [Test]
        public void Cohesion()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
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
            StreamReader reader = new StreamReader(resourcesPath + "test4.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            Piece ant1 = board.GetPiece(new Position(-1, -2, 0));
            Piece ant2 = board.GetPiece(new Position(1, 1, 0));
            Piece spider = board.GetPiece(new Position(1, 3, 0));

            Assert.AreEqual(Winner.None, board.CheckEndCondition());
            board.MovePiece(ant1, new Position(0, 1));
            Assert.AreEqual(Winner.Black, board.CheckEndCondition());
            board.MovePiece(ant2, new Position(-1, -2));
            Assert.AreEqual(Winner.White, board.CheckEndCondition());
            board.MovePiece(ant2, new Position(1, 1)); 
            board.MovePiece(ant1, new Position(-1, -2));
            board.MovePiece(spider, new Position(0, 1, 0));
            Assert.AreEqual(Winner.Draw, board.CheckEndCondition());
        }

        // Test the Queen Movement
        [Test]
        public void Queen()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test1.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
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
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
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
            StreamReader reader = new StreamReader(resourcesPath + "test1.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            Piece beetle = board.GetPiece(new Position(-1, -1, 0));
            List<Position> movements = board.GetMovements(beetle);
            Assert.AreEqual(4, movements.Count);
            Assert.IsTrue(movements.Contains(new Position(-1, 1, 0)));
            Assert.IsTrue(movements.Contains(new Position(-2, -2, 0)));
            Assert.IsTrue(movements.Contains(new Position(-1, 0, 1)));
            Assert.IsTrue(movements.Contains(new Position(-1, -3, 1)));
            
            Piece beetle2 = board.GetPiece(new Position(0, -2, 0));
            List<Position> movements2 = board.GetMovements(beetle2);
            Assert.AreEqual(5, movements2.Count);
            Assert.IsTrue(movements2.Contains(new Position(0, 0, 0)));
            Assert.IsTrue(movements2.Contains(new Position(0, -1, 0)));
            Assert.IsTrue(movements2.Contains(new Position(0, -3, 0)));
            Assert.IsTrue(movements2.Contains(new Position(0, -4, 0)));
            Assert.IsTrue(movements2.Contains(new Position(1, -3, 0)));
        }

        // Test the Ant Movement
        [Test]
        public void Ant()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            Piece ant = board.GetPiece(new Position(1, 1, 0));
            Assert.AreEqual(18, board.GetMovements(ant).Count);
        }

        // Test the Grasshopper Movement
        [Test]
        public void Grasshopper()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test3.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
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

        // Test the beginning of the Game
        // - First move
        // - Second move: next to the first piece
        // - Can't move pieces until the Queen is placed
        // - Must place the Queen in the fourth turn if isn't already placed (TODO)
        [Test]
        public void GameStart()
        {
            // Load an empty board
            StreamReader reader = new StreamReader(resourcesPath + "new.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            Piece queen1 = board.GetPiece(false, BugType.Queen, 0);
            Piece ant1 = board.GetPiece(false, BugType.Ant, 0);
            
            // First move
            List<Position> m1 = board.GetMovements(ant1);
            Assert.AreEqual(1, m1.Count);
            Assert.AreEqual(new Position(0, 0, 0), m1[0]);
            board.MovePiece(ant1, m1[0]);

            // Second move
            Piece ant2 = board.GetPiece(true, BugType.Ant, 0);
            List<Position> m2 = board.GetMovements(ant2);
            Assert.AreEqual(6, m2.Count);
            board.MovePiece(ant2, m2[0]);

            // Can't move until the queen is placed
            Assert.AreEqual(0, board.GetMovements(ant1).Count);
            List<Position> m3 = board.GetMovements(queen1);
            Assert.AreEqual(3, m3.Count);
            board.MovePiece(queen1, m3[0]);

            Piece spider2 = board.GetPiece(true, BugType.Spider, 0);
            board.MovePiece(spider2, board.GetMovements(spider2)[0]);

            Piece spider1 = board.GetPiece(false, BugType.Spider, 0);
            board.MovePiece(spider1, board.GetMovements(spider1)[4]);
            Assert.AreEqual(2, board.GetMovements(spider1).Count);

        }


    }
}