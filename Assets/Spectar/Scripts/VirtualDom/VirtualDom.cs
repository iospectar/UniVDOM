using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualDom
{
    public static VGameObject CreateGameObject(string name,  VComponent[] components, params VGameObject[] children)
    {
        return new VGameObject
        {
            name = name,
            components = components,
            children = children,
        };
    }

    public static VGameObject CreateGameObject(string name, params VGameObject[] children)
    {
        return new VGameObject
        {
            name = name,
            components = new VComponent[] { },
            children = children,
        };
    }

    public static VGameObject CreateGameObject(string name, VComponent[] components)
    {
        return new VGameObject
        {
            name = name,
            components = components,
            children = new VGameObject[] { },
        };
    }

    public static VGameObject CreateGameObject(string name)
    {
        return new VGameObject
        {
            name = name,
            components = new VComponent[] { },
            children = new VGameObject[] { },
        };
    }


    public static VComponent CreateComponent<T>(params KeyValuePair<string, object>[] fields)
    {
        return new VComponent
        {
            type = typeof(T),
            fields = fields,
        };
    }

    public static VComponent CreateComponent(Type type, params KeyValuePair<string, object>[] fields)
    {
        return new VComponent
        {
            type = type,
            fields = fields,
        };
    }

    public static VComponent CreateComponent(string type, params KeyValuePair<string, object>[] fields)
    {
        Type t = Type.GetType(type);
        return new VComponent
        {
            type = t,
            fields = fields,
        };
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
