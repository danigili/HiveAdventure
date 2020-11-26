using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using System.IO;

public class SaveGame
{
    private static SaveGame instance = null;

    private List<(int, int)> completedLevels;

    private static readonly fsSerializer serializer = new fsSerializer();
    private string filePath = Application.persistentDataPath + "/gamesave.json";
    private SaveGame()
    {
        Debug.Log(filePath);
        if (System.IO.File.Exists(filePath))
            Load();
        else
            completedLevels = new List<(int, int)>();
    }

    public static SaveGame GetInstance()
    {
        if (instance == null)
            instance = new SaveGame();
        return instance;
    }

    private void Load()
    {
        if (System.IO.File.Exists(filePath))
        {
            string text = System.IO.File.ReadAllText(filePath);
            fsData data = fsJsonParser.Parse(text);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(List<(int, int)>), ref deserialized).AssertSuccessWithoutWarnings();
            completedLevels = (List<(int, int)>)deserialized;
        }
        else
        {
            completedLevels = new List<(int, int)>();
        }
    }

    private void Save()
    {
        fsData data;
        serializer.TrySerialize(typeof(List<(int, int)>), completedLevels, out data).AssertSuccessWithoutWarnings(); // TODO : MIRAR AssertSuccessWithoutWarnings
        System.IO.File.WriteAllText(filePath, fsJsonPrinter.CompressedJson(data));
    }

    public void Reset()
    {
        File.Delete(filePath); 
        Load();
    }

    public void LevelCompleted(int zone, int level)
    {
        if (!completedLevels.Contains((zone, level)))
        {
            completedLevels.Add((zone, level));
        }
        Save();
    }

    public bool IsLevelCompleted(int zone, int level)
    {
        return completedLevels.Contains((zone, level));
    }

}
