using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very cool description of what this script does. Bear in mind that this documentation is mainly for the design team so they can install and play with the scripts that you made :)
/// Copy and paste this into your script and change the \<item\> objects
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
    /// A cool descripion of what this value does and how it effects the script and/or game world
    /// The tooltip attribute is very importent here so its easier for design to know what the value does
    /// </summary>
    [Range(0.0f, 100.0f), Tooltip("A cool descripion of what this value does and how it effects the script and/or game world")]
    public float sensitivity = 20.0f;

    [Tooltip("Set to true to see if you are happy")]
    private bool _happy = false;

    /// <summary>
    /// A very cool description of what the Happy varible does
    /// </summary>
    public bool Happy => _happy;
    
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

    /// <summary>
    /// Subtract two numbers, this will not show in the doxygen generated html. Therefore is optionol to document private members.
    /// </summary>
    /// <param name="a">the first number</param>
    /// <param name="b">the second number</param>
    /// <returns>a minus b</returns>
    private int SubtractNumbers(int a, int b)
    {
        return a - b;
    }
}
