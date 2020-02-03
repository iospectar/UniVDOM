using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class VCreate
{
    public static VGameObject CreateVirtualNodes(GameObject go)
    {
        VGameObject vGameObject = new VGameObject();

        List<VGameObject> children = new List<VGameObject>();

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform child = go.transform.GetChild(i);
            VGameObject vChild = CreateVirtualNodes(child.gameObject);
            children.Add(vChild);
        }

        vGameObject.name = go.name;
        vGameObject.children = children.ToArray();
        vGameObject.components = CreateVirtualComponents(go);

        return vGameObject;
    }

    private static VComponent[] CreateVirtualComponents(GameObject go)
    {
        List<VComponent> vComponents = new List<VComponent>();
        Component[] components = go.GetComponents<Component>();
        foreach(Component component in components)
        {
            vComponents.Add(CreateVirtualComponent(component));
        }
        return vComponents.ToArray();
    }

    private static VComponent CreateVirtualComponent(Component co)
    {
        VComponent vComponent = new VComponent();
        vComponent.fields = GetFields(co);
        vComponent.type = co.GetType();
        return vComponent;
    }

    private static KeyValuePair<string, object>[] GetFields(Component co)
    {
        List<KeyValuePair<string, object>> fields = new List<KeyValuePair<string, object>>();

        Type type = co.GetType();
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField;

        foreach(FieldInfo field in type.GetFields(bindingFlags))
        {
            try
            {
                object value = field.GetValue(co);
                fields.Add(new KeyValuePair<string, object>(field.Name, value));
            } catch(Exception e)
            {
                continue;
            }
        }

        foreach (PropertyInfo field in type.GetProperties(bindingFlags))
        {
            if (field.CanWrite)
            {
                try
                {
                    object value = field.GetValue(co);
                    fields.Add(new KeyValuePair<string, object>(field.Name, value));
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        return fields.ToArray();
    }
}
