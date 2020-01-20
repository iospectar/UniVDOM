using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RenderVirtualDom
{

    public static GameObject Render(VGameObject vGameObject)
    {
        GameObject go = new GameObject(vGameObject.name);

        foreach (VComponent vComponent in vGameObject.components)
        {
            Component component = RenderComponent(go, vComponent);
        }

        foreach (VGameObject vChild in vGameObject.children)
        {
            GameObject child = Render(vChild);
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

        foreach(KeyValuePair<string, object> field in vComponent.fields)
        {
            PropertyInfo myPropInfo = vComponent.type.GetProperty(field.Key);
            myPropInfo.SetValue(component, field.Value, null);
        }
        return component;
    }
}
