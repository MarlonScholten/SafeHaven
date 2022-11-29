using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by:  </br>
/// Interface for objects that can be triggered.
/// Objects with this interface can be added to the list of the triggerboxes.
/// </summary>
public interface ITrigger
{

    // Trigger the object
    public void trigger();
}