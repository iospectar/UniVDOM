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
        vApp = VCreate.CreateVirtualNodes(go, typeof(TodoItemPresenter), typeof(TextMesh), typeof(Transform));
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
        GameObjectPatch patch = VDiff.Diff(vApp, newApp);
        go = patch(go);
        vApp = newApp;
        count += 1;
    }


    VGameObject Render(List<TodoItem> todos)
    {
        var newApp = vApp.Clone();

        var todoContainer = newApp
            .Descendants()
            .First(go => go.name == "Todos");

        VGameObject[] todoItems = todos.Select(todo =>
            {
                int idx = todos.IndexOf(todo);
                VGameObject todoItem = VirtualDom.CreatePrefab("todo", todoItemPrefab,
                    VirtualDom.List(
             VirtualDom.CreateComponent<Transform>(new KeyValuePair<string, object>("position", new Vector3(0, idx, 0))),
                        VirtualDom.CreateComponent<TodoItemPresenter>(new KeyValuePair<string, object>("text", $"item {idx}"))
                    )
                );
                return todoItem;
            }).ToArray();

        todoContainer.children = todoItems;

        return newApp;
    }


    public void OnAddItem()
    {
        todos.Add(new TodoItem { text = "Item " + todos.Count });
        UpdateRender();
    }
}
