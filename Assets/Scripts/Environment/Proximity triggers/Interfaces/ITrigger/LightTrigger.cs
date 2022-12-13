using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </br>
/// Modified by:  </br>
/// This class implements the ITrigger interface and is used to activate a light in the scene.
/// When the trigger() method is called, the light will be activated.
/// How to add LightTrigger to scene:
/// 1. Add a light to the scene and turn it off.
/// 2. Add this script to the light.
/// 3. Create a trigger box of some kind to the scene.
/// 4. Add the TriggerObject script to the trigger box.
/// 5. Set the trigger box to IsTrigger.
/// 6. Add the light to the TriggerObject's TriggeredObjects list.
/// 7. When the player enters the trigger box, the light will be activated.
/// </summary>
public class LightTrigger : MonoBehaviour, ITrigger
{
    [SerializeField]
    private List<GameObject> _objectsInRange;

    private void Awake()
    {
        _objectsInRange = new();
    }

    /// <remarks>Param not optional.</remarks>
    /// <inheritdoc />
    public void TriggerEnter(GameObject instigator)
    {
        // Checks if the object doesn't live within the list, and adds it.
        if (!_objectsInRange.Contains(instigator))
            _objectsInRange.Add(instigator);

        // Set the state to active if it's not already, and there's more than 1 object.
        if (!gameObject.activeSelf && _objectsInRange.Count > 0)
            gameObject.SetActive(true);
    }

    /// <remarks>Param not optional.</remarks>
    /// <inheritdoc />
    public void TriggerExit(GameObject instigator)
    {
        // Checks if the object lives within the list, and removes it.
        if (_objectsInRange.Contains(instigator))
            _objectsInRange.Remove(instigator);

        // Set the state to inactive if it's not already, and there's less or equal to 0 objects.
        if (gameObject.activeSelf && _objectsInRange.Count <= 0)
            gameObject.SetActive(false);
    }
}