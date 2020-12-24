using System.IO;
using System.Xml;
using UnityEngine;

public static class ParseXml
{
    public static VGameObject Parse(string content)
    {
        var reader = new XmlTextReader(new StringReader(content));
        while(reader.Read())
        {
            if(reader.Name == "item1")
            {
                Debug.Log(reader.Name + " label = " + reader.GetAttribute("label"));
            }
        }

        return null;
    }
}
