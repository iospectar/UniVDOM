using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VGameObject vGameObject = Render();
        GameObject go = RenderVirtualDom.Render(vGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    VGameObject Render()
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject("parent",
            VirtualDom.List(
                VirtualDom.CreateComponent<TextMesh>(new KeyValuePair<string, object>("text", "Hello world"))
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
