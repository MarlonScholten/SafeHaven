using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
/// <summary>
/// Author: Jelco van der Straaten </br>
/// Modified by:  </br>
/// This script implements the ITrigger interface.
/// This object will be triggered when an object enters the triggerbox which has this object in its list.
/// This script can be attached to any gameobject, it will move upwards when triggered.
/// How to add TimelineTrigger to scene:
/// 1. Add a game object to the scene.
/// 2. Add this script to the game object.
/// 3. Create a trigger box of some kind to the scene.
/// 4. Add the TriggerObject script to the trigger box.
/// 5. Set the trigger box to IsTrigger.
/// 6. Add the game object to the TriggerObject's TriggeredObjects list.
/// 7. Select the PlayableDirector this trigger needs to start.
/// 8. Set a cooldown for the trigger.
/// 9. Set if the trigger can be used multiple times.
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Timeline prefab</term>
///		    <term>PlayableDirector</term>
///         <term>PlayableDirector</term>
///		    <term>The playableDirector will hold the cutscene</term>
///	    </item>
/// </list>
/// </summary>

[RequireComponent(typeof(TriggerObject))]
public class TimelineTrigger : MonoBehaviour, ITrigger
{
    /// <summary>
    /// This lists holds all the playableDirectors it will trigger.
    /// </summary>
    [SerializeField, Tooltip("This lists holds all the playableDirectors it will trigger.")]public List<PlayableDirector> playableDirectors;

    /// <summary>
    /// This bool checks if it is triggered already. Only used for one time triggers.
    /// </summary> 
    private bool _triggered = false;
    /// <summary>
    /// This bool checks if the trigger is on cooldown right now.
    /// </summary>
    private bool _onCoolDown = false;
    [SerializeField, Tooltip("True if the trigger can only be triggered once, false if you can trigger it multiple times.")] private bool _oneTimeTrigger = false;
    [SerializeField, Tooltip("The time in seconds that the trigger will not be triggered again if it has just been triggered.")] private int _coolDownSeconds = 20;

    
    /// <summary>
    /// This function gets called when a collider gets into the trigger zone.
    /// In this function it checks if it is on cooldown or if it is already triggered.
    /// If it is not, it will trigger the cutscene.
    /// </summary>
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

    /// <summary>
    /// This method create a cooldown for the selected ammount of seconds.
    /// </summary>
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_coolDownSeconds);
        _onCoolDown = false;
    }
}
