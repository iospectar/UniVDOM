using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Spectar.Scripts;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

public struct TodoItem
{
    public string text;
}

public class Sample : MonoBehaviour
{
    public GameObject root;

    VGameObject vApp;
    GameObject go;
    int count = 0;

    List<TodoItem> todos = new List<TodoItem>();


    // Start is called before the first frame update
    void Start()
    {
        vApp = VirtualDom.CreateGameObject(typeof(ListPresenter));
        go = root;
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
        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }


    VGameObject Render(List<TodoItem> todos)
    {
        var newApp = vApp.Clone();
        var list = VirtualDom.CreateGameObject(typeof(ListPresenter));
        newApp.children = new[] {list};
        list.children = todos.Select(todo =>
        {
            int idx = todos.IndexOf(todo);
            return VirtualDom.CreateGameObject(typeof(TodoItemPresenter), new []{new KeyValuePair<string, object>("Text", $"item {idx}")});
        }).ToArray();
        return newApp;
    }
    

    public void OnAddItem()
    {
        todos.Add(new TodoItem { text = "Item " + todos.Count });
        UpdateRender();
    }
}
