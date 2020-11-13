using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureMenu : MonoBehaviour
{
    public Dictionary<(int, int), Position> levelPos;

    public Pool buttons;

    private int zone;

    private void Start()
    {
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
        for (int i = 0; i < 6; i++)
        {
            AdventureButton button = buttons.GetInstance<AdventureButton>(true);
            Position pos;
            levelPos.TryGetValue((zone, i), out pos);
            button.Initialize(zone, i, false, false, pos, ButtonClick);
        }
    }

    public void BoardSize(out float xMin, out float xMax, out float yMin, out float yMax)
    {
        xMin = 1000;
        xMax = -1000;
        yMin = 1000;
        yMax = -1000;

        foreach (GameObject ho in buttons)
        {
            xMin = Math.Min(xMin, ho.transform.position.x);
            xMax = Math.Max(xMax, ho.transform.position.x);
            yMin = Math.Min(yMin, ho.transform.position.z);
            yMax = Math.Max(yMax, ho.transform.position.z);
        }
    }

    public void ButtonClick(AdventureButton button)
    { 
        
    }
}
