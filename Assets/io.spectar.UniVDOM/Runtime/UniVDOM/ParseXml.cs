using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Spectar.Scripts.VirtualDom;
using Unity.Profiling;
using UnityEngine;

public static class ParseXml
{
    static ProfilerMarker perfMarker = new ProfilerMarker("VirtualDom.ParseXml");

    public static VGameObject Parse(string xmlText, TagMap tagMap)
    {
        perfMarker.Begin();
        var doc = XDocument.Parse(xmlText);
        var root = doc.Root;
        var rootNode = NodeFactory(root, tagMap);
        CreateNode(rootNode, root, tagMap);
        perfMarker.End();
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
        var xAttributes = element.Attributes().ToList();
        var fields = new KeyValuePair<string, object>[xAttributes.Count];
        var attrIdx = 0;
        foreach (var xAttribute in xAttributes)
        {
            fields[attrIdx] = new KeyValuePair<string, object>(xAttribute.Name.LocalName, xAttribute.Value);
            attrIdx++;
        }
        var node = VirtualDom.CreateGameObject(t, fields);
        return node;
    }
}
