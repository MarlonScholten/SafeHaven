using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by: Hugo Verweij </br>
/// Interface for objects that can be triggered.
/// Objects with this interface can be added to the list of the triggerboxes.
/// </summary>
public interface ITrigger
{
    /// <summary>
    /// Trigger when an object enters that's not tagged with NonTrigger.
    /// </summary>
    /// <param name="instigator">The object that initiated the trigger.</param>
    /// <remarks>Param optional.</remarks>
    public void TriggerEnter(GameObject instigator = null);

    /// <summary>
    /// Trigger when an object exists that's not tagged with NonTrigger.
    /// </summary>
    /// <param name="instigator">The object that initiated the trigger.</param>
    /// <remarks>Method and param optional.</remarks>
    public void TriggerExit(GameObject instigator = null)
    {
        // Empty default implementation for objects that don't require it.
    }

}