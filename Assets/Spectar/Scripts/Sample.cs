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
        vApp = Render(0, Random.insideUnitSphere);
        go = VRender.RenderGameObject(vApp);
        InvokeRepeating("UpdateRender", 0, 1);
    }

    private void Update()
    {
        //UpdateRender();
    }

    // Update is called once per frame
    void UpdateRender()
    {
        VGameObject newApp = Render(count, Random.insideUnitSphere);
        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }

    VGameObject Render(int count, Vector3 pos)
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject("World",
            VirtualDom.List(
                VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", Vector3.zero)),
                VirtualDom.CreateComponent<TextMesh>(new KeyValuePair<string, object>("text", "Hello World " + count))
            ),
            VirtualDom.CreateGameObject("Child1",
                VirtualDom.List(
                    VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", pos))
                )
            ),
            VirtualDom.CreateGameObject("Child2", RenderList(count))
        );
        return vGameObject;
    }

    VGameObject[] RenderList(int length)
    {
        List<VGameObject> itemList = new List<VGameObject>();
        for (int i=0; i<length; i++)
        {
            Vector3 pos = new Vector3();
            pos.y = i;
            itemList.Add(RenderItem("item", pos, "Item " + i));
        }
        return itemList.ToArray();
    }

    VGameObject RenderItem(string name, Vector3 position, string text)
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject(name,
            VirtualDom.List(
                VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", position)),
                VirtualDom.CreateComponent<TextMesh>(new KeyValuePair<string, object>("text", text))
        ));
        return vGameObject;
    }
}
