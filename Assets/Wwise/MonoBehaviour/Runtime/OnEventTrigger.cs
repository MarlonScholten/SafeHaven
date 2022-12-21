using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Thomas van den OEver <br/>
/// Modified by:  <br/>
/// Description: A trigger function to call for sound events
/// </summary>
public class OnEventTrigger : AkTriggerBase
{
    private void Start()
    {
        //TODO: connect EventTrigger() to an event for loose coupeling maybe
    }

    public void EventTrigger()
    {
        triggerDelegate(null);
    }
}
