using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToolbarPresenter : MonoBehaviour
{
    private Button button;

    private UnityAction _onClick;

    public UnityAction OnClick
    {
        get
        {
            return _onClick;
        }

        set
        {
            if (button == null)
            {
                button = GetComponentInChildren<Button>();
            }

            if (value == null)
            {
                button.onClick.RemoveAllListeners();
            }
            else
            {
                button.onClick.AddListener(value);
            }
            _onClick = value;
        }
    }
}
