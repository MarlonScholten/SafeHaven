using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger
{
    GameObject triggerObject    {get;}
    public void trigger();
}