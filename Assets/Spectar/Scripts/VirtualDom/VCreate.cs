using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class VCreate
{
    // public static VGameObject CreateVirtualNodes(GameObject go, Type type)
    // {
    //     List<VGameObject> children = new List<VGameObject>();
    //
    //     for (int i = 0; i < go.transform.childCount; i++)
    //     {
    //         Transform child = go.transform.GetChild(i);
    //         VGameObject vChild = CreateVirtualNodes(child.gameObject, types);
    //         children.Add(vChild);
    //     }
    //
    //
    //     VGameObject vGameObject = new VGameObject(go.name, CreateVirtualComponents(go, types), children.ToArray());
    //
    //     return vGameObject;
    // }
    //
    // private static VComponent[] CreateVirtualComponents(GameObject go, params Type[] types)
    // {
    //     List<VComponent> vComponents = new List<VComponent>();
    //     Component[] components = go.GetComponents<Component>();
    //     foreach(Component component in components)
    //     {
    //         if (types.Length == 0)
    //         {
    //             vComponents.Add(CreateVirtualComponent(component));
    //         } else
    //         {
    //             if (Array.IndexOf(types, component.GetType()) > -1)
    //             {
    //                 vComponents.Add(CreateVirtualComponent(component));
    //             }
    //         }
    //     }
    //     return vComponents.ToArray();
    // }
    //
    // private static VComponent CreateVirtualComponent(Component co)
    // {
    //     VComponent vComponent = new VComponent(co.GetType(), GetFields(co));
    //     return vComponent;
    // }
    //
    // private static KeyValuePair<string, object>[] GetFields(Component co)
    // {
    //     List<KeyValuePair<string, object>> fields = new List<KeyValuePair<string, object>>();
    //
    //     Type type = co.GetType();
    //     const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField;
    //
    //     foreach(FieldInfo field in type.GetFields(bindingFlags))
    //     {
    //         try
    //         {
    //             object value = field.GetValue(co);
    //             fields.Add(new KeyValuePair<string, object>(field.Name, value));
    //         } catch(Exception e)
    //         {
    //             continue;
    //         }
    //     }
    //
    //     foreach (PropertyInfo field in type.GetProperties(bindingFlags))
    //     {
    //         if (field.CanWrite && field.Name != "material" && field.Name != "materials")
    //         {
    //             try
    //             {
    //                 object value = field.GetValue(co);
    //                 fields.Add(new KeyValuePair<string, object>(field.Name, value));
    //             }
    //             catch (Exception e)
    //             {
    //                 continue;
    //             }
    //         }
    //     }
    //
    //     return fields.ToArray();
    // }
}
