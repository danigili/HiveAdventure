﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameMain : MonoBehaviour
{
    public enum Stage
    { 
        Start,
        Game,
        Adventure,
        Pause
    }

    public BoardView boardView;
    public CameraController camera;
    private float angle = 60;
    public Stage stage = Stage.Start;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    public void Click1()
    {
        Debug.Log("HOLA");
        boardView.Initialize(BoardSerialization.FromFile("Text/Boards/new"));

    }

    public void Click2()
    {
        if (angle == 45)
            angle = 60;
        else if (angle == 60)
            angle = 90;
        else if (angle == 90)
            angle = 45;
        else
            angle = 60;
    }

    private void UpdateCameraPosition()
    {
        if (stage == Stage.Start)
        {
            camera.SetCenter(0, 0);
            camera.SetSize(2.6f);
        }
        else if (stage == Stage.Game)
        {
            if (boardView != null && boardView.pieces.Count > 0)
            {
                float xMin, xMax, yMin, yMax;
                boardView.BoardSize(out xMin, out xMax, out yMin, out yMax);
                camera.SetCenter((xMin + xMax) / 2, (yMin + yMax) / 2);
                camera.SetSize(Mathf.Max(Mathf.Max(xMax - xMin, yMax - yMin) * 0.5f + 1f, +5));
            }
            else
            {
                camera.SetCenter(1.558845f, 0);
                camera.SetSize(5);
            }
        }
        camera.SetAngle(angle);
    }
}
