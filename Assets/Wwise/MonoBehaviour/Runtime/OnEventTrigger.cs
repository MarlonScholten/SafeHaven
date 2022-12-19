using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
