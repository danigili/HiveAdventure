using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using UnityEngine.UIElements;

namespace Tests
{
    public class HiveAITests
    {
        private string resourcesPath = "./Assets/Tests/Resources/";

        [Test]
        public void BlackWinsOneMove()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test4.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            board.Initialize();
            AI.AIResult result = AI.FindBestMove(true, board, 0);
            board.MovePiece(result.move.piece, result.move.position);
            Assert.AreEqual(Winner.Black, board.CheckEndCondition());
        }

        [Test]
        public void WhiteWinsOneMove()
        {
            StreamReader reader = new StreamReader(resourcesPath + "test4.json");
            Board board = Serializer<Board>.FromJson(reader.ReadToEnd());
            board.Initialize();
            AI.AIResult result = AI.FindBestMove(false, board, 0);
            board.MovePiece(result.move.piece, result.move.position);
            Assert.AreEqual(Winner.White, board.CheckEndCondition());
        }
    }
}