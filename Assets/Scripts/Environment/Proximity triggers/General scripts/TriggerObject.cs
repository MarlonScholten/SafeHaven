using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by:  </br>
/// This script must be put on a triggerbox object.
/// The IsTrigger property must be set to true.
/// The list can be filled with objects that implement the ITrigger interface
/// When an object  (not with tag "NonTrigger") enters the triggerbox, the trigger() method of the ITrigger interface is called.
/// </summary>
public class TriggerObject : MonoBehaviour
{
    //list of serializable ITrigger objects
    [SerializeField] private List<GameObject> _triggerObjects;

    private void OnTriggerEnter(Collider other)
    {
        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object
        if (other.tag == "NonTrigger") return;
        foreach (var triggerObject in _triggerObjects)
        {
            if (triggerObject.GetComponent<ITrigger>() != null)
            {
                //trigger the object
                triggerObject.GetComponent<ITrigger>().trigger();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object
        if (other.tag == "NonTrigger") return;
        foreach (var triggerObject in _triggerObjects)
        {
            if (triggerObject.GetComponent<ITrigger>() != null)
            {
                //trigger the object
                triggerObject.GetComponent<ITrigger>().trigger();
            }
        }
    }
}
