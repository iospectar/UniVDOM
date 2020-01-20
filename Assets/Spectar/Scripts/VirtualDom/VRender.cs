using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class VRender
{

    public static GameObject RenderGameObject(VGameObject vGameObject)
    {
        GameObject go = new GameObject(vGameObject.name);

        foreach (VComponent vComponent in vGameObject.components)
        {
            Component component = RenderComponent(go, vComponent);
        }

        foreach (VGameObject vChild in vGameObject.children)
        {
            GameObject child = RenderGameObject(vChild);
            child.transform.SetParent(go.transform);
        }
        
        return go;
    }

    public static Component RenderComponent(GameObject go, VComponent vComponent)
    {
        Component component = go.GetComponent(vComponent.type);
        if (component == null)
        {
            component = go.AddComponent(vComponent.type);
        }

        
        return RenderFields(component, vComponent);
    }

    public static Component RenderFields(Component component, VComponent vComponent)
    {
        foreach (KeyValuePair<string, object> field in vComponent.fields)
        {
            SetComponentField(vComponent, component, field);
        }
        return component;
    }

    public static void SetComponentField(VComponent vComponent, Component component, KeyValuePair<string, object> field)
    {
        PropertyInfo myPropInfo = vComponent.type.GetProperty(field.Key);
        myPropInfo.SetValue(component, field.Value, null);
    }
}
