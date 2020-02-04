using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class VGameObject
{
    public string name;
    public GameObject prefab;
    public VComponent[] components;
    public VGameObject[] children;

    public VGameObject(string name, GameObject prefab, VComponent[] components, VGameObject[] children)
    {
        this.name = name;
        this.prefab = prefab;
        this.components = components;
        this.children = children;
    }

    public VGameObject(string name, VComponent[] components, VGameObject[] children)
    {
        this.name = name;
        this.components = components;
        this.children = children;
        this.prefab = null;
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

    public IEnumerable<VComponent> Components()
    {
        foreach (VComponent child in this.components)
        {
            yield return child;
        }
    }

    internal VGameObject Clone()
    {
        var name = this.name;
        var components = (VComponent[]) this.components
            .Select(component => component.Clone())
            .ToArray();

        var children = (VGameObject[]) this.children
            .Select(child => child.Clone())
            .ToArray();
        VGameObject clone = new VGameObject(name, components, children);
  
        return clone;
    }

    internal void UpdateChild(int idx, Func<VGameObject> cb)
    {
        children[idx] = cb();
    }
}
