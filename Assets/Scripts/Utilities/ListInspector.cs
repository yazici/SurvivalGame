using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomDictionaryItem
{
    [SerializeField]
    private string m_key;
    public string Key { get { return m_key; } set { m_key = value; } }
    [SerializeField]
    private UnityEngine.Object m_value;
    public UnityEngine.Object Value { get { return m_value; } set { m_value = value; } }
    public CustomDictionaryItem(string key, UnityEngine.Object value)
    {
        m_key = key;
        m_value = value;
    }
}

[Serializable]
public class CustomDictionary
{
    [SerializeField]
    private List<CustomDictionaryItem> l;

    public void Add(string key, UnityEngine.Object value)
    {
        l.Add(new CustomDictionaryItem(key, value));
    }
    public bool ContainsKey(string key)
    {
        return l.Any(sdi => sdi.Key == key);
    }

    public bool Remove(string key)
    {
        return l.RemoveAll(sdi => sdi.Key == key) != 0;
    }
    public UnityEngine.Object this[string key]
    {
        get
        {
            if (ContainsKey(key))
                return (UnityEngine.Object)l.First(sdi => sdi.Key == key).Value;
            return null;
        }
        set
        {
            if (ContainsKey(key))
            {
                CustomDictionaryItem item = l.First(sdi => sdi.Key == key);
                item.Value = value;
            }
            else
                Add(key, value);
        }
    }
    public List<string> Keys
    {
        get
        {
            return l.Select(sdi => sdi.Key).ToList();
        }
    }
    public List<UnityEngine.Object> Values
    {
        get
        {
            return l.Select(sdi => (UnityEngine.Object)sdi.Value).ToList();
        }
    }

    public List<CustomDictionaryItem>.Enumerator GetEnumerator()
    {
        return l.GetEnumerator();
    }
}