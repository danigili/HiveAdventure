using UnityEngine;
using FullSerializer;

public class Serializer<T>
{
    private static readonly fsSerializer serializer = new fsSerializer();

    public static T FromJson(string json)
    {
        fsData data = fsJsonParser.Parse(json);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        if (typeof(T) == typeof(Board))
            ((Board)deserialized).Initialize();
        return (T)deserialized;
    }

    public static T FromFile(string filePath)
    {
        TextAsset serializedState = Resources.Load<TextAsset>(filePath);
        return FromJson(serializedState.text);
    }

    public static string ToJson(T board)
    {
        fsData data;
        serializer.TrySerialize(typeof(T), board, out data).AssertSuccessWithoutWarnings(); // TODO : MIRAR AssertSuccessWithoutWarnings
        return fsJsonPrinter.CompressedJson(data);
    }
}
