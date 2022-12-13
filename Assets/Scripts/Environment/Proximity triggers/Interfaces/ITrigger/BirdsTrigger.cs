using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by: Hugo Verweij. </br>
/// This script implements the ITrigger interface.
/// This object will be triggered when an object enters the triggerbox which has this object in its list.
/// This script can be attached to any gameobject, it will move upwards when triggered.
/// How to add BirdsTrigger to scene:
/// 1. Add a game object to the scene.
/// 2. Add this script to the game object.
/// 3. Create a trigger box of some kind to the scene.
/// 4. Add the TriggerObject script to the trigger box.
/// 5. Set the trigger box to IsTrigger.
/// 6. Add the game object to the TriggerObject's TriggeredObjects list.
/// 7. When the player enters the trigger box, the game object will be fly up.
/// </summary>

public class BirdsTrigger : MonoBehaviour, ITrigger
{
    private bool _triggered = false; //bool to check if the object has been triggered
    [SerializeField] private float _timeBeforeDestroy = 5f; //time before the object is destroyed
    private float _timeTriggered = 0f; //time the object has been triggered

    /// <inheritdoc />
    public void TriggerEnter(GameObject instigator = null)
    {
        // The object is triggered
        _triggered = true;
       
        _timeTriggered = Time.time;
    }

    public void Update()
    {
        // if the object is triggered, move it upwards
        if (_triggered)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        // if the object is triggered and the time is up, destroy the object
        if (Time.time - _timeTriggered > _timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
