using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very cool description of what to do with this script and what it needs, what other components, tags, layers and/or scripts are needed for this script to work.
/// </summary>
/// <list type="Components, tags, layers and/or scripts">
/// <param name="componentname">This component need to be on the object or objects.</param>
/// <param name="tagname">This tag needs to be on the object or objects.</param>
/// <param name="scriptname">This script needs to be connected to the object or objects.</param>
/// </list>
public class DoxyTest : MonoBehaviour
{
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
