using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VGameObject vGameObject = VirtualDom.CreateGameObject("parent",
                VirtualDom.List(
                    VirtualDom.CreateComponent(typeof(Rigidbody), new KeyValuePair<string, object>("mass", 2.5f))
                ),
            VirtualDom.CreateGameObject("Child1"),
            VirtualDom.CreateGameObject("Child2")
       );

        GameObject go = RenderVirtualDom.Render(vGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
