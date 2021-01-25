using System.Collections;
using io.spectar.UniVDOM.Tests.Fixtures;
using NUnit.Framework;
using Spectar.Scripts.VirtualDom;
using UnityEngine.TestTools;

namespace io.spectar.UniVDOM.Tests.Runtime
{
    public class TestPatch
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestPatchSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestPatchWithEnumeratorPasses()
        {
            var tagMap = new TagMap();
            tagMap.Add("list", typeof(ListPresenter));
            tagMap.Add("todo", typeof(TodoItemPresenter));

            var content= $@"
            <list>
                <todo Text='Hello World 1'/>
                <todo Text='Hello World 2' />
            </list>
            ";            
            var vGameObject = ParseXml.Parse(content, tagMap);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
