using System.Linq;
using NUnit.Framework;
using Spectar.Scripts;
using Spectar.Scripts.VirtualDom;

namespace Spectar.Tests.Editor
{
    public class TestXmlRendering
    {

        // A Test behaves as an ordinary method
        [Test]
        public void TestXmlToVDom()
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
            Assert.AreEqual(1, vGameObject.children.First().fields.Length, "Should contain 1 text field");
        }
    }
}
