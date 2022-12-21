using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: <br/>
/// Description: Controller for a rest point ot determine if there is a bird on it.
/// </summary>
///<list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>BirdRestPoint</term>
///		    <term>GameObject</term>
///         <term>BirdRestPointVariables</term>
///		    <term>This script will hold a bool to determine if bird is on the rest point</term>
///	    </item>
/// </list>
public class BirdRestPointVariables : MonoBehaviour
{
    [Tooltip("Bool to check if there is a bird already on the rest point")] public bool isBirdOnRestPoint;
}
