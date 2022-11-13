using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    //list of serializable ITrigger objects
    [SerializeField, SerializeReference] private List<ITrigger> _triggerObjects;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(_triggerObjects.Count);
        foreach (var triggerObject in _triggerObjects)
        {
            triggerObject.trigger();
        }
    }
}
