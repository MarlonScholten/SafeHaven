using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    
    /// <summary>
    /// This value determines which objects with which tags may trigger this trigger.
    /// </summary>
    [SerializeField,TagSelector, Tooltip("Select the tags of game object you want to trigger this trigger with.")] private String[] tags = new string[]{};


    private void OnTriggerEnter(Collider other)
    {
        var tagEquals = false;
        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object
        foreach (var checkTag in tags)
        {
            if (checkTag.Equals(other.tag))
            {
                tagEquals = true;
            }
        }
        // If no tags equalled a tag in the list, the method returns.
        if (tagEquals == false) return;
        
        foreach (var triggerObject in _triggerObjects.Where(triggerObject => triggerObject.GetComponent<ITrigger>() != null))
        {
            //trigger the object
            triggerObject.GetComponent<ITrigger>().trigger();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var tagEquals = false;
        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object
        foreach (var checkTag in tags)
        {
            if (checkTag.Equals(other.tag))
            {
                tagEquals = true;
            }
        }
        if (tagEquals == false) return;
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
