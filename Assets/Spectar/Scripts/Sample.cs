using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class Sample : MonoBehaviour
{
    VGameObject vApp;
    GameObject go;
    int count = 200;
    // Start is called before the first frame update
    void Start()
    {
        vApp = Render(0, UnityEngine.Random.insideUnitSphere);
        go = VRender.RenderGameObject(vApp);
        InvokeRepeating("UpdateRenderThreaded", 0, 1f);
    }

    private void Update()
    {
        //UpdateRender();
    }

    void UpdateRenderThreaded()
    {
        VGameObject newApp = Render(count, UnityEngine.Random.insideUnitSphere);
        IObservable<GameObjectPatch> result = VDiff.DiffThreaded(vApp, newApp);
        Observable.WhenAll(result)
            .ObserveOnMainThread()
            .Subscribe(patch =>
            {
                go = patch[0](go);
                vApp = newApp;
                count += 1;
            })
            .AddTo(this);
    }

    void UpdateRender()
    {
        VGameObject newApp = Render(count, UnityEngine.Random.insideUnitSphere);
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
            VirtualDom.CreateGameObject("List", Enumerable.Range(0, count).Select(
                row =>
                {
                    return VirtualDom.CreateGameObject("item",
                        VirtualDom.List(
                            VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", new Vector3(0, row, 0))),
                            VirtualDom.CreateComponent<TextMesh>(new KeyValuePair<string, object>("text", "hello world " + row))
                    ));
                }
            ))
        );
        return vGameObject;
    }
}
