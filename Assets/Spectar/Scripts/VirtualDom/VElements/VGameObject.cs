using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class VGameObject
{
    public GameObject prefab;
    public VGameObject[] children;

    readonly public Type type;
    readonly public KeyValuePair<string, object>[] fields;
    
    public void SetValue(string key, object value)
    {
        int idx = Array.FindIndex(fields, kv => kv.Key == key);
        fields[idx] = new KeyValuePair<string, object>(key, value);
    }
    
    public bool IsType<T>()
    {
        return type == typeof(T);
    }

    public VGameObject(string typeName, KeyValuePair<string, object>[] fields, VGameObject[] children)
    {
        this.type = Type.GetType(typeName);
        this.children = children;
        this.fields = fields;
    }
    
    public VGameObject(Type type, KeyValuePair<string, object>[] fields, VGameObject[] children)
    {
        this.type = type;
        this.children = children;
        this.fields = fields;
    }


    public void Replace(VGameObject current, VGameObject replace)
    {
        int idx = Array.IndexOf(children, current);
        children[idx] = replace;
    }

    public IEnumerable<VGameObject> Descendants()
    {
        foreach(VGameObject child in children)
        {
            yield return child;

            foreach(VGameObject desc in child.Descendants())
            {
                yield return desc;
            }
        }
    }


    internal VGameObject Clone()
    {
        var fields = this.fields
            .Select(field => new KeyValuePair<string, object>(field.Key, field.Value))
            .ToArray();

        var children = (VGameObject[]) this.children
            .Select(child => child.Clone())
            .ToArray();
        VGameObject clone = new VGameObject(type, fields, children);
  
        return clone;
    }

    internal void UpdateChild(int idx, Func<VGameObject> cb)
    {
        children[idx] = cb();
    }
}
