using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

public struct TodoItem
{
    public string text;
}

public class Sample : MonoBehaviour
{
    public GameObject toolbarPrefab;
    public GameObject todoItemPrefab;

    VGameObject vApp;
    GameObject go;
    int count = 0;

    List<TodoItem> todos = new List<TodoItem>();


    // Start is called before the first frame update
    void Start()
    {
        go = gameObject.transform.GetChild(0).gameObject;
        vApp = VCreate.CreateVirtualNodes(go);
        Debug.Log(vApp);
        //go = VRender.RenderGameObject(vApp);
        //go.transform.SetParent(transform);

        //InvokeRepeating("UpdateRenderThreaded", 0, 2f);
    }

    private void Update()
    {
        //UpdateRender();
    }

    void UpdateRenderThreaded()
    {
        VGameObject newApp = Render(todos);
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
        VGameObject newApp = Render(todos);
        string val1 = (string) vApp.children[0].components[1].fields[0].Value;
        string val2 = (string) newApp.children[0].components[1].fields[0].Value;

        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }


    VGameObject Render(List<TodoItem> todos)
    {
        var newApp = vApp.Clone();

        newApp
            .Descendants()
            .Where(go => go.name == "TodoItem")
            .First()
            .Components()
            .Where(co => co.IsType<TodoItemPresenter>())
            .First()
            .SetValue("text", todos.Count().ToString());

        return newApp;
    }


    public void OnAddItem()
    {
        todos.Add(new TodoItem { text = "Item " + todos.Count });
        UpdateRender();
    }
}
