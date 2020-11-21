using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMenu : MonoBehaviour
{
    public Text progressText;
    public GameObject endPanel;

    public Dictionary<(int, int), Position> levelPos;

    public Pool buttons;

    private int zone;
    private int maxZone;
    private SaveGame save;
    private BoardView boardView;


    private void Start()
    {
        save = SaveGame.GetInstance();
        levelPos = Serializer<Dictionary<(int, int), Position>>.FromFile("Adventure/ButtonsPosition");
        boardView = gameObject.GetComponent<BoardView>();
    }

    public void DrawButtons(int zone)
    {
        this.zone = zone;
        buttons.ClearAll();
        maxZone = 10;
        bool completed = true;
        for (int z = 0; z <= maxZone; z++)
        {
            for (int i = 0; i < 6; i++)
            {
                Position pos;
                if (!levelPos.TryGetValue((z, i), out pos))
                {
                    maxZone = z-1;
                    break;
                }

                AdventureButton button = buttons.GetInstance<AdventureButton>(true);
                button.Initialize(z, i, save.IsLevelCompleted(z,i), completed, pos, ButtonClick);
                completed = save.IsLevelCompleted(z, i);
            }
        }
        progressText.text = GetProgress();
    }

    public void ZoneUp()
    {
        zone++;
        if (zone > maxZone) zone = maxZone;
    }

    public void ZoneDown()
    {
        zone--;
        if (zone < 0) zone = 0;
    }

    public void Clear()
    {
        foreach (GameObject go in buttons)
            go.GetComponent<AdventureButton>().Clear();
    }

    public void BoardSize(out float xMin, out float xMax, out float yMin, out float yMax)
    {
        xMin = 1000;
        xMax = -1000;
        yMin = 1000;
        yMax = -1000;

        foreach (GameObject ho in buttons)
        {
            if (ho.GetComponent<AdventureButton>().zone == this.zone)
            {
                xMin = Math.Min(xMin, ho.transform.position.x);
                xMax = Math.Max(xMax, ho.transform.position.x);
                yMin = Math.Min(yMin, ho.transform.position.z);
                yMax = Math.Max(yMax, ho.transform.position.z);
            }
        }
    }

    public String GetProgress()
    {
        int completedLevels = 0;
        for (int z = 0; z <= maxZone; z++)
            for (int i = 0; i < 6; i++)
                if (save.IsLevelCompleted(z, i))
                    completedLevels++;

        return  String.Format("{0:0}%", completedLevels / ((maxZone + 1f) * 6f) *100f);
    }

    public void ButtonClick(AdventureButton button)
    {
        //save.LevelCompleted(button.zone, button.level);
        boardView.Initialize(Serializer<Board>.FromFile("Adventure/Levels/Zone" + button.zone + "/Level" + button.level), EndOfGame);
        
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

    }
}
