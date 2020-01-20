using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate GameObject GameObjectPatch(GameObject go);
public delegate Component ComponentPatch(GameObject co);
public delegate void FieldsPatch(Component co);

public class VDiff
{

    private static FieldsPatch DiffFields(VComponent vOldComponent, VComponent vNewComponent)
    {
        FieldsPatch patch = (Component co) =>
        {
            for (int i = 0; i < vOldComponent.fields.Length; i++)
            {
                KeyValuePair<string, object> field = vNewComponent.fields[i];
                bool unchanged = vOldComponent.fields[i].Value.Equals(field.Value);
                if (!unchanged)
                {
                    VRender.SetComponentField(vNewComponent, co, field);
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
            VGameObject? newChild = null;
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

    private static GameObjectPatch DiffComponents(VComponent[] vOldComponents, VComponent[] vNewComponents)
    {
        GameObjectPatch patch = (GameObject go) =>
        {
            int minLength = Mathf.Min(vOldComponents.Length, vNewComponents.Length);

            for (int i = 0; i < minLength; i++)
            {
                ComponentPatch componentPatch = DiffComponent(vOldComponents[i], vNewComponents[i]);
                componentPatch(go);
            }

            // patch is complete
            if (vOldComponents.Length == vNewComponents.Length)
            {
                return go;

                // New components have beend added
            }
            else if (vOldComponents.Length < vNewComponents.Length)
            {
                for (int i = minLength - 1; i < vNewComponents.Length; i++)
                {
                    ComponentPatch componentPatch = DiffComponent(null, vNewComponents[i]);
                    componentPatch(go);
                }

            // Components have been deleted
            }
            else if (vOldComponents.Length > vNewComponents.Length)
            {
                for (int i = minLength - 1; i < vOldComponents.Length; i++)
                {
                    ComponentPatch componentPatch = DiffComponent(vOldComponents[i], null);
                    componentPatch(go);
                }
            }
            return go;
        };

        return patch;
    }

    private static ComponentPatch DiffComponent(VComponent? vOldComponent, VComponent? vNewComponent)
    {
        ComponentPatch patch = (GameObject go) =>
        {
            FieldsPatch fieldPatch = DiffFields(vOldComponent.Value, vNewComponent.Value);
            Component co = go.GetComponent(vOldComponent.Value.type);
            fieldPatch(co);
            return co;
        };

        // If there is no new component then we're gonna delete it
        if (!vNewComponent.HasValue && vOldComponent.HasValue)
        {
            patch = (GameObject go) =>
            {
                Component co = go.GetComponent(vOldComponent.Value.type);
                GameObject.Destroy(co);
                return null;
            };
        }

        // If there's a new one, but no old one, we're adding a new one
        VComponent vNotNullNewCO = vNewComponent.Value;
        if (!vOldComponent.HasValue)
        {
            patch = (GameObject go) =>
            {
                return VRender.RenderComponent(go, vNotNullNewCO);
            };
        }

        // If the components of different types, then we're gonna replace
        // the old with the new
        VComponent vNotNullOldCO = vOldComponent.Value;
        if (!vNotNullOldCO.type.Equals(vNotNullNewCO.type))
        {
            patch = (GameObject go) =>
            {
                Component co = go.GetComponent(vNotNullOldCO.type);
                GameObject.Destroy(co);
                return VRender.RenderComponent(go, vNotNullNewCO);
            };
        }

        return patch;
    }

    public static GameObjectPatch Diff(VGameObject vOldGO, VGameObject? vNewGO)
    {
        GameObjectPatch patch = (GameObject go) => null;

        // GameObject has been deleted
        if (!vNewGO.HasValue)
        {
            patch = (GameObject go) => {
                GameObject.Destroy(go);
                return null;
            };
        }

        VGameObject vNotNullGO = vNewGO.Value;
        if (vOldGO.name != vNotNullGO.name)
        {
            patch = (GameObject go) =>
            {
                Transform parent = go.transform.parent;
                GameObject.Destroy(go);
                GameObject newGo = VRender.RenderGameObject(vNotNullGO);
                newGo.transform.SetParent(parent);
                return newGo;
            };
        }

        patch = (GameObject go) =>
        {
            GameObjectPatch componentPatch = DiffComponents(vOldGO.components, vNotNullGO.components);
            componentPatch(go);
            GameObjectPatch childPatch = DiffChildren(vOldGO.children, vNotNullGO.children);
            childPatch(go);
            return go;
        };
        return patch;
    }
}
