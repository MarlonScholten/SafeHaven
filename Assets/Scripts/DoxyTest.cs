using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very cool description of what to do with this script and what it needs, what other components, tags, layers and/or scripts are needed for this script to work.
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
///     <item>
///		    <term>The debugging GameObject</term>
///	     	<term>Script</term>
///	       	<term>DebugPosition.cs</term>
///	       	<term>Usefull for the debugger to know the position of the player</term>
///	    </item>
///	    <item>
///		    <term>Child GameObject of this GameObject</term>
///		    <term>Tag</term>
///		    <term>Friendly</term>
///		    <term>Used because this script looks for these tags to see if it applies to them</term>
///	    </item>
///	    <item>
///		    <term>Parent GameObject of this GameObject</term>
///		    <term>Layer</term>
///		    <term>non_interacteble</term>
///		    <term>Used so the pinging system doens't ping the player itself</term>
///		</item>
/// </list>
/// 




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

    /// <summary>
    /// Checks if a number is less than 1
    /// </summary>
    /// <example>
    /// 
    /// </example>
    /// <param name="number">The number to be checked</param>
    /// <returns>returns 0 if OK, -1 if Error</returns>
    private int CheckIfTiny(double number)
    {
        if(number < 1)
        {
            return 0;
        }
        return -1;
    }
}
