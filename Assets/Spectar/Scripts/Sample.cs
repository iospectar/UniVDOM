using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    VGameObject vApp;
    GameObject go;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        vApp = Render("hello world", Random.insideUnitSphere);
        go = VRender.RenderGameObject(vApp);
        InvokeRepeating("UpdateRender", 0, 2);
    }

    // Update is called once per frame
    void UpdateRender()
    {
        VGameObject newApp = Render("hello world " + count, Random.insideUnitSphere);
        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }

    VGameObject Render(string text, Vector3 pos)
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject("World",
            VirtualDom.List(
                VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", pos)),
                VirtualDom.CreateComponent<TextMesh>(new KeyValuePair<string, object>("text", text))
            ),
            VirtualDom.CreateGameObject("Child1",
                VirtualDom.List(
                    VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", new Vector3(1, 2, 3)))
                )
            ),
            VirtualDom.CreateGameObject("Child2")
        );
        return vGameObject;
    }
}
