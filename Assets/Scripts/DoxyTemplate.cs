using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very cool description of what this script does. Bear in mind that this documentation is mainly for the design team so they can install and play with the scripts that you made :)
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>The GameObject this thing need to be on for this script to work</term>
///		    <term>What type of this is. Component, Script, Tag or Layer?</term>
///         <term>The name of thing</term>
///		    <term>A discription of why this thing is need</term>
///	    </item>
///	    <item>
///         <term>The same GameObject as this script is on</term>
///		    <term>Component</term>
///         <term>CharacterController</term>
///		    <term>Script uses the move() function of the character controller to move the player</term>
///	    </item>
/// </list>

public class DoxyTest : MonoBehaviour
{
    /// <summary>
    /// Set to true to see if you are happy
    /// </summary>
    [SerializeField]
    public bool happy = false;

    /// <summary>
    /// Used for the sensitivity of the mouse, ranges form 0 to 100
    /// </summary>
    [Range(0.0f, 100.0f)]
    public float sensitivity = 20.0f;
    
    /// <summary>
    /// Adds two numbers togheter
    /// </summary>
    /// <param name="a">The first number to be added</param>
    /// <param name="b">The second number to be added</param>
    /// <returns>The sum of a and b</returns>
    public int AddNumbers(int a, int b)
    {
        return a + b;
    }
}
