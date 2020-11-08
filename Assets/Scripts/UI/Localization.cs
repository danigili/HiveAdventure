using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Language
{ 
    CA,
    ES,
    EN
}

public class Localization
{
    private static Language firstLanguage;
    private static Language secondtLanguage;
    private static Dictionary<string, string> firstDictionary;
    private static Dictionary<string, string> secondDictionary;

    private static Dictionary<string, string> LoadLanguageFile(Language lan)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        TextAsset asset = Resources.Load<TextAsset>("Translate/" + lan.ToString());
        string[] lines = asset.text.Split('\n');
        foreach (string line in lines)
        {
            string[] pair = line.Split('=');
            if (pair.Length == 2)
                dic.Add(pair[0], pair[1].Replace("\\n","\n"));
        }
        return dic;
    }

    public static void SetLanguage(Language first, Language second = Language.EN)
    {
        firstDictionary = LoadLanguageFile(first);
        secondDictionary = LoadLanguageFile(second);
    }

    public static Language GetCurrentLanguage()
    {
        return firstLanguage;
    }

    public static string Translate(string key)
    {
        string result = null;
        if (!firstDictionary.TryGetValue(key, out result))
            result = secondDictionary[key];
        return result;
    }
}
