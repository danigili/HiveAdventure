using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoardView : MonoBehaviour
{
    public Pool piecesPool;
    public Pool markersPool;
    public Pool rocksPool;
    public Pool chainsPool;
    private Board model;
    public PieceObject selectedPiece;
    public PieceUI selectedUIPiece;
    public PiecesPanel[] panels;
    public bool ai1 = false;
    public bool ai2 = true;
    public bool currentSide = false;
    private Action<Winner> endCallback;
    private bool finished = false;
    private AudioSource audioSource;
    public AudioClip[] soundArray;
    public int turnsCounter;
    public Hourglass hourglass;
    public BackgroundClick bg;

    public void Initialize(Board board, Action<Winner> endCallback, bool side = false)
    {
        bg.Initialize(ClickDownBackground);
        this.model = board;
        this.currentSide = side;
        this.endCallback = endCallback;
        this.finished = false;
        Dictionary<(int, int, int), Piece> placedPieces = model.GetPlacedPieces();
        List<Piece> notPlacedPieces = model.GetNotPlacedPieces();

        turnsCounter = board.GetMaxTurns();
        if (turnsCounter > 0)
        {
            hourglass.gameObject.SetActive(true);
            hourglass.SetValue(turnsCounter, false);
        }
        else
        {
            turnsCounter = -1;
        }

        piecesPool.ClearAll();
        foreach (KeyValuePair<(int, int, int), Piece> pair in placedPieces)
        {
            GameObject instance = piecesPool.GetInstance(true);
            instance.GetComponent<PieceObject>().Initialize(pair.Value, ClickDown);
            instance.GetComponent<PieceObject>().x = pair.Key.Item1;
            instance.GetComponent<PieceObject>().y = pair.Key.Item2;
            instance.GetComponent<PieceObject>().z = pair.Key.Item3;
            if (pair.Value.blocked)
            {
                GameObject chainInstance = chainsPool.GetInstance(true);
                chainInstance.GetComponent<HexObject>().SetHexPosition(pair.Value.position);
            }
        }

        foreach (Position p in model.GetBlockedPositions())
        {
            GameObject instance = rocksPool.GetInstance(true);
            instance.GetComponent<HexObject>().SetHexPosition(p);
        }

        panels[0].Initialize(model, ClickPanelPiece);
        panels[1].Initialize(model, ClickPanelPiece);
        panels[0].transform.parent.GetComponent<Animator>().SetBool("show", true);

        audioSource = GetComponent<AudioSource>();

        markersPool.ClearAll();
        selectedPiece = null;
        selectedUIPiece = null;
    }

    // Hide the panels removes the game
    public void Clear()
    {
        panels[0].transform.parent.GetComponent<Animator>().SetBool("show", false);
        piecesPool.ClearAll();
        markersPool.ClearAll();
        hourglass.gameObject.SetActive(false);
        selectedPiece = null;
        selectedUIPiece = null;
    }

    public void Prueba2()
    {
        Debug.Log(Serializer<Board>.ToJson(model));
        DateTime inicio = DateTime.Now;
        for (int i = 0; i < 100000; i++)
            model.BreaksCohesion(model.GetPlacedPieces().First().Value);
        Debug.Log((DateTime.Now - inicio).Minutes + ":" + (DateTime.Now - inicio).Seconds + "." + (DateTime.Now - inicio).Milliseconds);
    }

    public IEnumerator AIMove(bool side)
    {
        yield return new WaitForSeconds(1);
        //DateTime inicio = DateTime.Now;
        // Find best move
        AI.AIResult ai = AI.FindBestMove(side, model, 0);
        //Debug.Log((DateTime.Now - inicio).Minutes + ":" + (DateTime.Now - inicio).Seconds + "." + (DateTime.Now - inicio).Milliseconds);
        //Debug.Log("Leaves: " + ai.leaves + ", Value: " + ai.bestValue);

        // Find piece to move in placed pieces
        foreach (PieceObject po in piecesPool.Next<PieceObject>())
        {
            if (po.piece.Equals(ai.move.piece) && po.gameObject.activeSelf)
            {
                model.MovePiece(ai.move.piece, ai.move.position);
                audioSource.PlayOneShot(soundArray[UnityEngine.Random.Range(0, soundArray.Length - 1)]);
                Position newPos = model.GetPiecePosition(po.piece);
                po.SetHexPosition(ai.move.position.x, ai.move.position.y, ai.move.position.z);
                selectedPiece = null;
                selectedUIPiece = null;
                break;
            }
        }

        // Find piece to move in the panel
        foreach (PiecesPanel panel in panels)
        {
            foreach (PieceUI pui in panel.pieces)
            {
                if (pui.piece.Equals(ai.move.piece))
                {
                    model.MovePiece(ai.move.piece, ai.move.position);
                    audioSource.PlayOneShot(soundArray[UnityEngine.Random.Range(0, soundArray.Length - 1)]);
                    GameObject instance = piecesPool.GetInstance(true);
                    instance.GetComponent<PieceObject>().Initialize(ai.move.piece, ClickDown);
                    instance.GetComponent<PieceObject>().SetHexPosition(model.GetPiecePosition(ai.move.piece));
                    panel.RemovePiece(pui);
                    selectedPiece = null;
                    selectedUIPiece = null;
                    break;
                }
            }
        }

        selectedPiece = null;
        selectedUIPiece = null;
        NextTurn();
    }

    public void Evaluate()
    {
        Debug.Log(model.EvaluateBoard());
    }

    /**********   Click callbacks   ***********/
    public void ClickPanelPiece(PieceUI piece)
    {
        if (finished) return;

        // Ignore if managed by AI or is not your turn
        if (piece.piece.side != currentSide || (ai1 && !currentSide) || (ai2 && currentSide))
            return;

        List<Position> positions = model.GetMovements(piece.piece);

        markersPool.ClearAll();
        if (piece.Equals(selectedUIPiece))
        {
            selectedUIPiece = null;
            return;
        }
        selectedPiece = null;
        selectedUIPiece = piece;
        for (int i = 0; i < positions.Count; i++)
        {
            Marker marker = markersPool.GetInstance<Marker>(true);
            marker.Initialize(ClickDownMarker);
            marker.SetHexPosition(positions[i]);
        }
    }

    public void ClickDown(PieceObject piece)
    {
        if (finished) return;

        // Ignore if managed by AI or is not your turn
        if (piece.piece.side != currentSide || (ai1 && !currentSide) || (ai2 && currentSide))
            return;

        List<Position> positions = model.GetMovements(piece.piece);

        markersPool.ClearAll();
        if (piece.Equals(selectedPiece))
        {
            selectedPiece = null;
            return;
        }
        selectedPiece = piece;
        selectedUIPiece = null;
        for (int i = 0; i < positions.Count; i++)
        {
            Marker marker = markersPool.GetInstance<Marker>(true);
            marker.Initialize(ClickDownMarker);
            marker.SetHexPosition(positions[i]);
        }
    }

    public void ClickDownMarker(Marker marker)
    {
        if (selectedPiece != null)
        {
            model.MovePiece(selectedPiece.piece, (marker.x, marker.y, marker.z));
            audioSource.PlayOneShot(soundArray[UnityEngine.Random.Range(0, soundArray.Length - 1)]);
            Position newPos = model.GetPiecePosition(selectedPiece.piece);
            selectedPiece.SetHexPosition(newPos.x, newPos.y, newPos.z);
            selectedPiece = null;
        }
        else if (selectedUIPiece != null)
        {
            if (selectedUIPiece.piece.side)
                panels[1].RemovePiece(selectedUIPiece);
            else
                panels[0].RemovePiece(selectedUIPiece);
            model.MovePiece(selectedUIPiece.piece, (marker.x, marker.y, marker.z));
            audioSource.PlayOneShot(soundArray[UnityEngine.Random.Range(0, soundArray.Length - 1)]);
            Position newPos = model.GetPiecePosition(selectedUIPiece.piece);
            GameObject instance = piecesPool.GetInstance(true);
            instance.GetComponent<PieceObject>().Initialize(selectedUIPiece.piece, ClickDown);
            instance.GetComponent<PieceObject>().SetHexPosition(model.GetPiecePosition(selectedUIPiece.piece));
            selectedUIPiece = null;
        }

        NextTurn();
    }

    public void ClickDownBackground()
    {
        markersPool.ClearAll();
        selectedPiece = null;
        selectedUIPiece = null;
    }

    private void NextTurn()
    {
        markersPool.ClearAll();

        // I the match has end, execute end of game callback
        Winner winner = model.CheckEndCondition();
        if (winner != Winner.None)
        {
            finished = true;
            endCallback(winner);
            return;
        }

        // Change player
        currentSide = !currentSide;

        // In adventure mode, decrement turns counter
        if (currentSide)
        {
            turnsCounter--;
            hourglass.SetValue(turnsCounter);
        }

        // If counter == 0, end game
        if (turnsCounter == 0)
        {
            finished = true;
            endCallback(Winner.None);
            return;
        }

        // If player cannot move, pass the turn to the other player
        if (!model.CanTakeTurn(currentSide))
            currentSide = !currentSide;

        // If the player is controlled by AI, execute AI method
        if ((!currentSide && ai1) || (currentSide && ai2))
            StartCoroutine(AIMove(currentSide));

    }

    /****   AUXILIAR   ***/
    public void BoardSize(out float xMin, out float xMax, out float yMin, out float yMax)
    {
        xMin = 1000;
        xMax = -1000;
        yMin = 1000;
        yMax = -1000;

        foreach (PieceObject p in piecesPool.Next<PieceObject>())
        {
            xMin = Math.Min(xMin, p.GetWorldPosition().x);
            xMax = Math.Max(xMax, p.GetWorldPosition().x);
            yMin = Math.Min(yMin, p.GetWorldPosition().z);
            yMax = Math.Max(yMax, p.GetWorldPosition().z);
        }
    }
}
