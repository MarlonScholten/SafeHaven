using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class TimelineTrigger : MonoBehaviour, ITrigger
{
    public List<PlayableDirector> playableDirectors;

    private bool _triggered = false;
    private bool _onCoolDown = false;
    [SerializeField, Tooltip("True if the trigger can only be triggered once, false if you can trigger it multiple times.")] private bool _oneTimeTrigger = false;
    [SerializeField, Tooltip("The time in seconds that the trigger will not be triggered again if it has just been triggered.")] private int _coolDownSeconds = 20;

    public void trigger()
    {
        if (_triggered || _onCoolDown) return;
        //Setting the triggered to true is it is only a one time trigger
        if (_oneTimeTrigger)
        {
            _triggered = true;
        }
        
        foreach (var playableDirector in playableDirectors)
        {
            playableDirector.Play();
        }
        
        //Starting the cooldown
        _onCoolDown = true;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_coolDownSeconds);
        _onCoolDown = false;
    }
}
