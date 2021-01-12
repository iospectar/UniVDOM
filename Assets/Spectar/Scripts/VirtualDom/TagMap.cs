using System;
using System.Collections.Generic;

namespace Spectar.Scripts.VirtualDom
{
    public class TagMap
    {
        private readonly Dictionary<string, Type> tagMap = new Dictionary<string, Type>();

        public void Add(string tag, Type type)
        {
            tagMap[tag] = type;
        }
        
        public Type Get(string tag)
        {
            return tagMap[tag];
        }
    }
}