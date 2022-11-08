using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuggableObject
{
    public Dictionary<string, string> GetDebugValues();
}
