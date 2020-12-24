using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestXmlRendering
    {

        private string RenderTodo(string text)
        {
            return $@"
            <Todo>
                <Components>
                    <Transform position='[1,2,3]' />
                    <TodoItemPresenter text='{{text}}' />
                </Components>
            </Todo>
            ";
        }
        
        private string RenderTodos(int count)
        {
            var enumerable = Enumerable.Range(0, count).Select(c => c.ToString());
            return $@"
            <Todos>
                {enumerable.Select(RenderTodo)}
            </Todos>
            ";
        }

        // A Test behaves as an ordinary method
        [Test]
        public void TestXmlToVDom()
        {
            var content = RenderTodos(4);
            var xmlString = ParseXml.Parse(content);
            // Use the Assert class to test conditions
        }
    }
}
