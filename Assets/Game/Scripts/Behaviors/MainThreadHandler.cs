using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadHandler : MonoBehaviour
{
    private List<Action> _mainThreadActionList = new List<Action>();

    public void AddMainThreadAction(Action action)
    {
        _mainThreadActionList.Add(action);
    }

    private void Update()
    {
        if (_mainThreadActionList.Count<=0)
        {
            return;
        }

        _mainThreadActionList[0]?.Invoke();
        _mainThreadActionList.RemoveAt(0);
    }
}