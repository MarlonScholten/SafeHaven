using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script implements the ITrigger interface.
/// This object will be triggered when an object enters the triggerbox which has this object in its list.
/// This script can be attached to any gameobject, it will move upwards when triggered.
/// </summary>

/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by:  </para>
/// This script implements the ITrigger interface.
/// This object will be triggered when an object enters the triggerbox which has this object in its list.
/// This script can be attached to any gameobject, it will move upwards when triggered.
/// </summary>

public class BirdsTrigger : MonoBehaviour, ITrigger
{
    private bool _triggered = false;

    //trigger the object
    public void trigger()
    {
        // The object is triggered
        _triggered = true;
    }

    public void Update()
    {
        // if the object is triggered, move it upwards
        if (_triggered)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1 * Time.deltaTime, transform.position.z);
        }
    }
}
