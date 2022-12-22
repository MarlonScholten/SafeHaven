using System.Collections;
using System.Collections.Generic;
using SoundManager;
using UnityEngine;

public class LampSound : SoundBase, ITrigger
{
    [SerializeField] private List<GameObject> _objectsInRange;

    private bool _isOn  = false;

    private void Awake()
    {
        _objectsInRange = new();
    }

    public void TriggerEnter(GameObject instigator)
    {
        // Checks if the object doesn't live within the list, and adds it.
        if (!_objectsInRange.Contains(instigator))
            _objectsInRange.Add(instigator);

        if (_isOn) return;
        
        PlayAudioOn();
        
        _isOn = true;
    }

    public void TriggerExit(GameObject instigator)
    {
        // Checks if the object lives within the list, and removes it.
        if (_objectsInRange.Contains(instigator))
            _objectsInRange.Remove(instigator);

        // Set the state to inactive if it's not already, and there's less or equal to 0 objects.
        if (_objectsInRange.Count <= 0)
        {
            PlayAudioOff();
            _isOn = false;
        }
    }

    private void PlayAudioOn()
    {
        playSound(0, gameObject);
        playSound(1, gameObject);
    }

    private void PlayAudioOff()
    {
        stopSound(0);
        playSound(1, gameObject);
    }
}
