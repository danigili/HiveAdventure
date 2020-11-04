using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public BoardView boardView;
    public CameraController camera;
    public TextAsset level;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }
    private void UpdateCameraPosition()
    {
        if (boardView != null && boardView.piecesPool.Count() > 0)
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
        camera.SetAngle(60);
    }

    public void LoadLevel()
    {
        boardView.Initialize(BoardSerialization.FromJson(level.text), EndOfGame);
    }

    public void EndOfGame(Winner winner)
    { 
        
    }
}
