using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Spectar.Scripts.VirtualDom;
using UnityEngine;

public static class ParseXml
{
    private static string ViewNameSpace = "Spectar.Scripts";
    public static VGameObject Parse(string xmlText, TagMap tagMap)
    {
        var doc = XDocument.Parse(xmlText);
        var root = doc.Root;
        var rootNode = NodeFactory(root, tagMap);
        CreateNode(rootNode, root, tagMap);
        return rootNode;
    }

    private static void CreateNode(VGameObject node, XElement element, TagMap tagMap)
    {
        var elements = element.Elements().ToArray();
        var children = new VGameObject[elements.Length];
        var childIdx = 0;
        foreach (var xElement in elements)
        {
            var child = NodeFactory(xElement, tagMap);
            children[childIdx] = child;
            CreateNode(child, xElement, tagMap);
            childIdx++;
        }
        node.children = children;
    }

    private static VGameObject NodeFactory(XElement element, TagMap tagMap)
    {
        var tagName = element.Name.LocalName;
        var t = tagMap.Get(tagName);
        var node = VirtualDom.CreateGameObject(t, new KeyValuePair<string, object>[0]);
        return node;
    }
}
