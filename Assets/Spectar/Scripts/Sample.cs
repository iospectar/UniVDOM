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
        vApp = Render("hello world");
        go = VRender.RenderGameObject(vApp);
        //InvokeRepeating("UpdateRender", 0, 4);
    }

    private void Update()
    {
        UpdateRender();
    }

    // Update is called once per frame
    void UpdateRender()
    {
        VGameObject newApp = Render("hello world " + count);
        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }

    VGameObject Render(string text)
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject("parent",
            VirtualDom.List(
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
