using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by: Hugo Verweij, Thomas van den Oever </br>
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
        //Debug.Log($"Collider hit ENTER {other.name}");

        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object

        if (!tags.Contains(other.tag)) return;
        
        foreach (var triggerObject in _triggerObjects.Where(triggerObject => triggerObject.GetComponent<ITrigger>() != null))
        {
            //trigger the object
            triggerObject.GetComponent<ITrigger>().TriggerEnter(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"Collider hit EXIT {other.name}");

        //if the object that enters the triggerbox is not tagged as "NonTrigger" and implements the ITrigger interface, trigger the object

        if (!tags.Contains(other.tag)) return;

        foreach (var triggerObject in _triggerObjects)
        {
            if (triggerObject.GetComponent<ITrigger>() != null)
            {
                //trigger the object
                triggerObject.GetComponent<ITrigger>().TriggerExit(other.gameObject);
            }
        }
    }
}
