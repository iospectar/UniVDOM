using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class VRender
{

    public static GameObject RenderGameObject(VGameObject vGameObject)
    {
        GameObject go;
        if (vGameObject.prefab == null)
        {
            go = new GameObject(vGameObject.type.ToString());
        } else
        {
            go = GameObject.Instantiate(vGameObject.prefab);
            go.name = vGameObject.type.ToString();
        }

        RenderComponent(go, vGameObject);

        // Prefabs create their own children
        if (vGameObject.prefab == null)
        {
            foreach (VGameObject vChild in vGameObject.children)
            {
                GameObject child = RenderGameObject(vChild);
                child.transform.SetParent(go.transform);
            }
        }

        return go;
    }

    public static GameObject RenderComponent(GameObject go, VGameObject vGameObject)
    {
        Component component = go.GetComponent(vGameObject.type);
        if (component == null)
        {
            component = go.AddComponent(vGameObject.type);
        }

        
        return RenderFields(go, vGameObject);
    }

    public static GameObject RenderFields(GameObject go, VGameObject vComponent)
    {
        foreach (KeyValuePair<string, object> field in vComponent.fields)
        {
            if (field.Key.ToLower() == "material")
            {
                Debug.Log("Material");
            }
            SetField(vComponent, go, field);
        }
        return go;
    }

    public static void SetField(VGameObject vGameObject, GameObject go, KeyValuePair<string, object> field)
    {
        FieldInfo fieldInfo = vGameObject.type.GetField(field.Key);
        Component component = go.GetComponent(vGameObject.type);
        if (fieldInfo == null)
        {
            PropertyInfo myPropInfo = vGameObject.type.GetProperty(field.Key);
            myPropInfo.SetValue(component, field.Value);
        } else
        {
            fieldInfo.SetValue(component, field.Value);
        }
    }
}
