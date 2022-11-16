using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the ITrigger interface and is used to activate a light in the scene when the player enters a trigger object
/// </summary>
public class Light : MonoBehaviour, ITrigger
{

    public void trigger()
    {
        //set object active
        gameObject.SetActive(!gameObject.activeSelf);
    }
}