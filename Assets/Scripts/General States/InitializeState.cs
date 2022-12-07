using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Author: Iris Giezen<br/>
/// Modified by: Marlon Kerstens<br/>
/// The start state can be used to initialize the state machine.
/// It waits a frame so that everything can be instantiated.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Object with a visual scripting state machine</term>
///		    <term>Component</term>
///         <term>Object with a visual scripting state machine</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT methods for the Initialize states.</term>
///	    </item>
///	    <item>
///         <term>Object with a visual scripting state machine</term>
///		    <term>Visual scripting</term>
///         <term>Visual scripting graph</term>
///		    <term>This script need to be added to the visual scripting graph node.</term>
///	    </item>
/// </list>
public class InitializeState : MonoBehaviour
{
    /// <summary>
    /// The enter method for the initialize state, it waits a frame until it goes to the start state.
    /// </summary>
    public void InitializeEnter()
    {
        StartCoroutine(WaitForNextFrame());
    }

    /// <summary>
    /// The update method for the initialize state.
    /// </summary>
    public void InitializeUpdate()
    {
    }

    /// <summary>
    /// The Fixed update method for the initialize state.
    /// </summary>
    public void InitializeFixedUpdate()
    {
    }

    /// <summary>
    /// The exit method for the initialize state.
    /// </summary>
    public void InitializeExit()
    {
    }

    private IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
        CustomEvent.Trigger(this.gameObject, "Start");
    }
}