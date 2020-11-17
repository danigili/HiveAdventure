using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureMenu : MonoBehaviour
{
    public Dictionary<(int, int), Position> levelPos;

    public Pool buttons;

    private int zone;
    private int maxZone;
    private SaveGame save;

    private void Start()
    {
        save = SaveGame.GetInstance();
        levelPos = Serializer<Dictionary<(int, int), Position>>.FromFile("Adventure/ButtonsPosition");
        //StartCoroutine(Prueba());
    }

    public IEnumerator Prueba()
    { 
        yield return new WaitForSeconds(1);
        DrawButtons(0);
        Debug.Log("AA");
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

    public void ButtonClick(AdventureButton button)
    {
        save.LevelCompleted(button.zone, button.level);
        DrawButtons(0);
    }
}
