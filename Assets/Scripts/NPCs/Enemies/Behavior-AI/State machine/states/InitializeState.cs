using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Author: Iris Giezen </para>
/// Modified by: N/A </para>
/// The start state of the EnemyAI state machine.
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
///         <term>EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab)</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT methods for the Initialize states.</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Visual scripting</term>
///         <term>Enemy_Visual_Scripting (Assets/Scripts/NPCs/Enemies/Behavior-AI/State machine/visualScripting/Enemy_Visual_Scripting.asset)</term>
///		    <term>This script need to be added to the EnemyObject with the Enemy_Visual_Scripting</term>
///	    </item>
/// </list>
public class InitializeState : MonoBehaviour
{
    /// <summary>
    /// The enter method for the initialize state, it waits a frame until it goes to the patrol state.
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
        CustomEvent.Trigger(this.gameObject, "Patrol");
    }
}