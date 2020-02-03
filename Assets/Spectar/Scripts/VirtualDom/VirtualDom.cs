using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirtualDom
{
    public static VGameObject CreateGameObject(string name,  VComponent[] components, params VGameObject[] children)
    {
        return new VGameObject
        (
            name,
            components,
            children
        );
    }

    public static VGameObject CreateGameObject(string name, params VGameObject[] children)
    {
        return new VGameObject
        (
            name,
            new VComponent[] { },
            children
        );
    }

    public static VGameObject CreateGameObject(string name, IEnumerable<VGameObject> children)
    {
        return new VGameObject(
            name,
            new VComponent[0],
            children.ToArray()
        );
    }

    public static VGameObject CreatePrefab(string name, GameObject prefab)
    {
        return new VGameObject
        (
            name,
            prefab,
            new VComponent[0],
            new VGameObject[0]
        );
    }

    public static VGameObject CreatePrefab(string name, GameObject prefab, VComponent[] components)
    {
        return new VGameObject
        (
            name,
            prefab,
            components,
            new VGameObject[0]
        );
    }

    public static VGameObject CreateGameObject(string name, VComponent[] components)
    {
        return new VGameObject
        (
            name,
            components,
            new VGameObject[0]
        );
    }

    public static VGameObject CreateGameObject(string name)
    {
        return new VGameObject
        (
            name,
            new VComponent[0],
            new VGameObject[0]
        );
    }


    public static VComponent CreateComponent<T>(params KeyValuePair<string, object>[] fields)
    {
        return new VComponent
        (
            typeof(T),
            fields
        );
    }

    public static VComponent CreateComponent(Type type, params KeyValuePair<string, object>[] fields)
    {
        return new VComponent
        (
            type,
            fields
        );
    }

    public static VComponent CreateComponent(string type, params KeyValuePair<string, object>[] fields)
    {
        Type t = Type.GetType(type);
        return new VComponent
        (
            t,
            fields
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
