using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VGameObject
{
    public string name;
    public GameObject prefab;
    public VComponent[] components;
    public VGameObject[] children;
}
