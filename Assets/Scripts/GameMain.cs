﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public enum Stage
    { 
        Start,
        Mode,
        Game,
        Adventure,
        Pause,
        End
    }

    public BoardView boardView;
    public CameraController camera;
    private float angle = 60;
    public Stage stage = Stage.Start;
    public GameObject startPanel;
    public GameObject integratedUI;
    public GameObject endPanel;
    private float endTimer = 0;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Localization.SetLanguage(Language.CA);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
        UpdateStartMenu();
        UpdateEndOfGame();
        endTimer -= Time.deltaTime;
    }

    private IEnumerator RestartGame()
    {
        if (stage == Stage.Game)
        {
            boardView.Clear();
            yield return new WaitForSeconds(1);
            boardView.Initialize(BoardSerialization.FromFile("Text/Boards/new"), EndOfGame);
        }
    }

    public void CameraClick()
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
        if (stage == Stage.Start || stage == Stage.Mode)
        {
            camera.SetCenter(0, 0, false);
            camera.SetSize(2.6f, false);
        }
        else if (stage == Stage.Game)
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
            }
        }
        camera.SetAngle(angle);
    }

    private void UpdateStartMenu()
    {
        if (stage == Stage.Start)
        {
            startPanel.SetActive(true);

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                stage = Stage.Mode;
                startPanel.SetActive(false);
                integratedUI.GetComponent<Animator>().SetBool("show", true);
            }
        }
    }

    private void UpdateEndOfGame()
    {
        if (stage == Stage.End && endTimer < 0)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                endPanel.GetComponent<Animator>().SetBool("show", false);
                stage = Stage.Game;
                StartCoroutine(RestartGame());
            }
        }
    }

    public IEnumerator IntegratedButtonClick(string option)
    {
        if (option == "quick")
        {
            integratedUI.GetComponent<Animator>().SetBool("show", false);
            yield return new WaitForSeconds(0.5f);
            stage = Stage.Game;
            boardView.Initialize(BoardSerialization.FromFile("Text/Boards/new"), EndOfGame);

        }
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<Animator>().SetBool("show", true);
    }

    public void CloseSettings()
    {
        settingsMenu.GetComponent<Animator>().SetBool("show", false);
    }

    public void Continue()
    {
        ClosePause();
    }

    public void Restart()
    {
        StartCoroutine(RestartGame());
        ClosePause();
    }

    public void Exit()
    {
        integratedUI.GetComponent<Animator>().SetBool("show", true);
        boardView.Clear();
        ClosePause();
        stage = Stage.Mode;
    }

    public void PauseClick()
    {
        OpenPause();
    }

    public void OpenPause()
    {
        pauseMenu.SetActive(true);
        pauseMenu.GetComponent<Animator>().SetBool("show", true);
    }

    public void ClosePause()
    {
        pauseMenu.GetComponent<Animator>().SetBool("show", false);
    }

    public void EndOfGame(Winner winner)
    {
        endPanel.SetActive(true);
        endPanel.GetComponent<Animator>().SetBool("show", true);
        if (winner == Winner.Black)
            endPanel.transform.Find("Text").GetComponent<Text>().text = Localization.Translate("BLACK_WINS");
        else if (winner == Winner.White)
            endPanel.transform.Find("Text").GetComponent<Text>().text = Localization.Translate("WHITE_WINS");
        else
            endPanel.transform.Find("Text").GetComponent<Text>().text = Localization.Translate("DRAW");

        stage = Stage.End;
        endTimer = 1;
    }
}
