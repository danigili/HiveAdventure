using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HiveModelTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void HiveModelSimplePasses()
        {
            Board board = new Board();
            board.Initialize();
            board.MovePiece(board.GetNotPlacedPieces()[0], (0, 0, 0));

            Assert.IsTrue(board.GetPlacedPieces().ContainsKey((0, 0, 0)));
            // TODO: CUANDO PUEDA CARGAR Y GUARDAR JUGADAS DESDE JSON, TENDRÉ QUE DESARROLLAR LOS TEST
        }

    }
}
