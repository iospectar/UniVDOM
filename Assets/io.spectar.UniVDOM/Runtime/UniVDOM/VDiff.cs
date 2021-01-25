using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public delegate GameObject GameObjectPatch(GameObject go);
public delegate Component ComponentPatch(GameObject co);
public delegate void FieldsPatch(GameObject co);

public class VDiff
{

    private static FieldsPatch DiffFields(VGameObject vOldGameObject, VGameObject vNewGameObject)
    {
        FieldsPatch patch = (GameObject go) =>
        {
            for (int i = 0; i < vOldGameObject.fields.Length; i++)
            {
                KeyValuePair<string, object> field = vNewGameObject.fields[i];
                object currentValue = vOldGameObject.fields[i].Value;
                bool unchanged;
                if (currentValue == null)
                {
                    unchanged = currentValue == field.Value;
                } else
                {
                    unchanged = currentValue.Equals(field.Value);
                }

                if (!unchanged)
                {
                    VRender.SetField(vNewGameObject, go, field);
                }
            }
        };
        return patch;
    }

    private static GameObjectPatch DiffChildren(VGameObject[] vOldChildren, VGameObject[] vNewChildren)
    {
        List<GameObjectPatch> patches = new List<GameObjectPatch>();
        for(int i=0; i<vOldChildren.Length; i++)
        {
            VGameObject newChild = null;
            if (i<vNewChildren.Length)
            {
                newChild = vNewChildren[i];
            }
            patches.Add(Diff(vOldChildren[i], newChild));
        }

        List<GameObjectPatch> additionalPatches = new List<GameObjectPatch>();
        if (vNewChildren.Length > vOldChildren.Length)
        {
            for (int i = vOldChildren.Length; i < vNewChildren.Length; i++)
            {
                VGameObject newChild = vNewChildren[i];
                GameObjectPatch newChildPatch = (GameObject go) =>
                {
                    GameObject newGo = VRender.RenderGameObject(newChild);
                    newGo.transform.SetParent(go.transform);
                    return newGo;
                };
                additionalPatches.Add(newChildPatch);
            }
        }

        GameObjectPatch completePatch = (GameObject go) =>
        {
            for(int i=0; i<patches.Count; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                patches[i](child);
            }

            foreach(GameObjectPatch patch in additionalPatches)
            {
                patch(go);
            }
            return go;
        };

        return completePatch;
    }
    
    public static GameObjectPatch Diff(VGameObject vOldGO, VGameObject vNewGO)
    {
        GameObjectPatch patch;

        // GameObject has been deleted
        if (vNewGO == null)
        {
            patch = (GameObject go) => {
                GameObject.Destroy(go);
                return null;
            };
        }
        // GameObject has been added
        else if (vOldGO == null)
        {
            patch = (GameObject go) =>
            {
                GameObject newGo = VRender.RenderGameObject(vNewGO);
                newGo.transform.SetParent(go.transform);
                return newGo;
            };
        }
        // GameObject has been modified
        else
        {
            patch = (GameObject go) =>
            {
                if (vOldGO.type != vNewGO.type)
                {
                    var oldComponent = go.GetComponent(vOldGO.type);
                    GameObject.DestroyImmediate(oldComponent);
                    go.AddComponent(vNewGO.type);
                }

                var componentPatch = DiffFields(vOldGO, vNewGO);
                componentPatch(go);
                GameObjectPatch childPatch = DiffChildren(vOldGO.children, vNewGO.children);
                childPatch(go);
                return go;
            };
        }
        return patch;
    }

    public static Task<GameObjectPatch> DiffThreaded(VGameObject vOldGO, VGameObject vNewGO)
    {
        return Task.Run(() => Diff(vOldGO, vNewGO));
    }
}
