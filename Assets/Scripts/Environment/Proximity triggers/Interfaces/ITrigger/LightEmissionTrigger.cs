using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightEmissionTrigger : MonoBehaviour, ITrigger
{
    [SerializeField]
    private List<GameObject> _objectsInRange;

    [SerializeField] private Material _materialOff;
    [SerializeField] private Material _MaterialOn;

    private void Awake()
    {
        _objectsInRange = new();
    }

    public void TriggerEnter(GameObject instigator)
    {
        // Checks if the object doesn't live within the list, and adds it.
        if (!_objectsInRange.Contains(instigator))
            _objectsInRange.Add(instigator);

        // Set the state to active if it's not already, and there's more than 1 object.
        if (_objectsInRange.Count > 0)
        {
            GetComponent<Renderer>().material = _MaterialOn;
        }
    }

    public void TriggerExit(GameObject instigator)
    {
        // Checks if the object lives within the list, and removes it.
        if (_objectsInRange.Contains(instigator))
            _objectsInRange.Remove(instigator);

        // Set the state to inactive if it's not already, and there's less or equal to 0 objects.
        if (_objectsInRange.Count <= 0)
            GetComponent<Renderer>().material = _materialOff;
    }
}
