using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;

public class DualDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    [NonSerialized]
    private Dictionary<TValue, TKey> secondDictionary = null;

    public DualDictionary() : base() 
    {
        ComputeSecondDictionary();
    }

    public DualDictionary(IDictionary<TKey, TValue> dictionary):base(dictionary) 
    {
        ComputeSecondDictionary();
    }

    public TKey getKey(TValue value) 
    {
        if (secondDictionary == null || secondDictionary.Count != this.Count)
            ComputeSecondDictionary();
        if (secondDictionary.ContainsKey(value))
            return secondDictionary[value];
        return default(TKey);
    }

    private void ComputeSecondDictionary()
    {
        if (secondDictionary == null)
            secondDictionary = new Dictionary<TValue, TKey>();
        foreach (KeyValuePair<TKey, TValue> pair in this)
            secondDictionary.Add(pair.Value, pair.Key);
    }

    public new bool Remove(TKey key)
    {
        if (secondDictionary == null || secondDictionary.Count != this.Count)
            ComputeSecondDictionary();
        secondDictionary.Remove(this[key]);
        return base.Remove(key);
    }

    public new void Add(TKey key, TValue value)
    {
        if (secondDictionary == null || secondDictionary.Count != this.Count)
            ComputeSecondDictionary();
        secondDictionary.Add(value, key);
        base.Add(key, value);
    }

    public new bool ContainsValue(TValue value) 
    {
        if (secondDictionary == null || secondDictionary.Count != this.Count)
            ComputeSecondDictionary();
        return secondDictionary.ContainsKey(value);
    }

}
