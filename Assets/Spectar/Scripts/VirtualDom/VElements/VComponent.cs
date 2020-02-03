using System;
using System.Collections.Generic;
using System.Linq;

public class VComponent
{
    readonly public Type type;
    readonly public KeyValuePair<string, object>[] fields;

    public VComponent(Type type, KeyValuePair<string, object>[] fields)
    {
        this.type = type;
        this.fields = fields;
    }

    public void SetValue(string key, object value)
    {
        int idx = Array.FindIndex(fields, kv => kv.Key == key);
        fields[idx] = new KeyValuePair<string, object>(key, value);
    }

    public VComponent Clone()
    {
        var fields = this.fields
            .Select(field => new KeyValuePair<string, object>(field.Key, field.Value))
            .ToArray();
        VComponent clone = new VComponent(type, fields);
        return clone;
    }

    public bool IsType<T>()
    {
        return type == typeof(T);
    }
}
