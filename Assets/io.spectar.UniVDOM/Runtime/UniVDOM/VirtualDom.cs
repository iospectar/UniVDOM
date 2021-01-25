using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirtualDom
{
    public static VGameObject CreateGameObject(string name,  KeyValuePair<string, object>[] fields, params VGameObject[] children)
    {
        return new VGameObject
        (
            name,
            fields,
            children
        );
    }

    public static VGameObject CreateGameObject(string name, params VGameObject[] children)
    {
        return new VGameObject
        (
            name,
            new KeyValuePair<string, object>[] { },
            children
        );
    }

    public static VGameObject CreateGameObject(Type type, KeyValuePair<string, object>[] fields)
    {
        return new VGameObject
        (
            type,
            fields,
            new VGameObject[0]
        );
    }

    public static VGameObject CreateGameObject(string name)
    {
        return new VGameObject
        (
            name,
            new KeyValuePair<string, object>[0],
            new VGameObject[0]
        );
    }
    
    public static VGameObject CreateGameObject(Type type)
    {
        return new VGameObject
        (
            type,
            new KeyValuePair<string, object>[0],
            new VGameObject[0]
        );
    }


    /// <summary>
    /// This is meant to just clean up the factory syntax a little
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static T[] List<T>(params T[] items)
    {
        return items;
    }

}
