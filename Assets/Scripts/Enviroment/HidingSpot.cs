using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by:  Jelco</para>
/// This script only stores the grade and obscurity values for the hiding spot.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
/// </list>


public class HidingSpot : MonoBehaviour
{
    /// <summary>
    /// This value describes the grade for the spot.
    /// </summary> 
    [SerializeField, Tooltip("This value describes the grade for the spot.")] public float _grade;

    /// <summary>
    /// This value gets determined by the hidingspot script. It is a combination of the grade, distance and visibility of the enemy to the spot.
    /// </summary> 
    [SerializeField, Tooltip("This value gets determined by the hidingspot script. It is a combination of the grade, distance and visibility of the enemy to the spot.")] public float _obscurityValue;

}
