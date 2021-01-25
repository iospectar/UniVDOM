using System.Collections;
using io.spectar.UniVDOM.Tests.Fixtures;
using NUnit.Framework;
using Spectar.Scripts.VirtualDom;
using UnityEngine;
using UnityEngine.TestTools;

namespace io.spectar.UniVDOM.Tests.Runtime
{
    public class TestPatch
    {
        private readonly TagMap tagMap = new TagMap();
        private VGameObject vApp = null;
        private GameObject root;
        
        [SetUp]
        public void Setup()
        {
            root = new GameObject("root");
            tagMap.Add("list", typeof(ListPresenter));
            tagMap.Add("todo", typeof(TodoItemPresenter));
        }
        

        [UnityTest]
        public IEnumerator TestRenderPatch()
        {
            var content= $@"
            <list>
            </list>
            ";
            
            Render(content);            
            yield return null;
            
            // A root list component should be created
            var rootComponent = root.GetComponent<ListPresenter>();
            Assert.IsNotNull(rootComponent, "The root component should be a list");
            Assert.AreEqual(0, root.transform.childCount, "And to start, that list should have no children");

            content= $@"
            <list>
                <todo></todo>
            </list>
            ";
            Render(content);
            yield return null;
            
            // The list should now have one child
            Assert.AreEqual(1, root.transform.childCount, "One child of the list should now be added");

            var text = "Hello world";
            content= $@"
            <list>
                <todo></todo>
                <todo Text='{text}'></todo>
            </list>
            ";
            Render(content);
            yield return null;
            
            // The list should now have two children and the second child should have the Text property set
            Assert.AreEqual(2, root.transform.childCount, "There should now be two children");
            var todoPresenter = root.transform.GetChild(1).GetComponent<TodoItemPresenter>();
            Assert.AreEqual(text, todoPresenter.Text);
            
            content= $@"
            <list>
                <todo Text='{text}'></todo>
            </list>
            ";
            Render(content);
            yield return null;
            
            // The list should now have one child and the child should have the Text property set
            Assert.AreEqual(1, root.transform.childCount, "There should now be two children");
            todoPresenter = root.transform.GetChild(0).GetComponent<TodoItemPresenter>();
            Assert.AreEqual(text, todoPresenter.Text);
        }

        private void Render(string content)
        {
            var newApp = ParseXml.Parse(content, tagMap);
            GameObjectPatch patch = VDiff.Diff(vApp, newApp);
            root = patch(root);
            vApp = newApp;
        }
    }
}
