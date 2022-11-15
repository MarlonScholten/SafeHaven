using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to activate a light in the scene when the player enters a trigger object
/// </summary>
public class TriggerObject : MonoBehaviour
{
    //list of serializable ITrigger objects
    [SerializeField] private List<GameObject> _triggerObjects;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
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
