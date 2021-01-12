using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodoItemPresenter : MonoBehaviour
{
    private string text;

    private TextMesh textMesh;

    public string Text
    {
        get => text;
        set
        {
            text = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // textMesh = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        // textMesh.text = text;
    }
}
