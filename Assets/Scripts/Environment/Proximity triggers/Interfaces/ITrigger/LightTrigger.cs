using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by:  </para>
/// This class implements the ITrigger interface and is used to activate a light in the scene.
/// When the trigger() method is called, the light will be activated.
/// An object with this script can be added to the list of the triggerboxes.
/// </summary>
public class LightTrigger : MonoBehaviour, ITrigger
{
    //trigger the object
    public void trigger()
    {
        //set object active
        gameObject.SetActive(!gameObject.activeSelf);
    }
}