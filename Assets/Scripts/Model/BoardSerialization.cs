using UnityEngine;
using FullSerializer;

public class BoardSerialization
{
    private static readonly fsSerializer serializer = new fsSerializer();

    public static Board FromJson(string json)
    {
        fsData data = fsJsonParser.Parse(json);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(Board), ref deserialized).AssertSuccessWithoutWarnings();
        ((Board)deserialized).Initialize();
        return (Board)deserialized;
    }

    public static Board FromFile(string filePath)
    {
        TextAsset serializedState = Resources.Load<TextAsset>(filePath);
        return FromJson(serializedState.text);
    }

    public static string ToJson(Board board)
    {
        fsData data;
        serializer.TrySerialize(typeof(Board), board, out data).AssertSuccessWithoutWarnings(); // TODO : MIRAR AssertSuccessWithoutWarnings
        return fsJsonPrinter.CompressedJson(data);
    }
}
