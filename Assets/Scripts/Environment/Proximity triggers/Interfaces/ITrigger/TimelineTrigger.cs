using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineTrigger : MonoBehaviour, ITrigger
{
    public List<PlayableDirector> playableDirectors;

    private bool _triggered = false;

    public void trigger()
    {
        _triggered = !_triggered;
        foreach (var playableDirector in playableDirectors)
        {
            playableDirector.Play();
        }
    }
}
