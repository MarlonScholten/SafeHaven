using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script is a the Alerted state of the enemy.
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
///		    <term>What type of thing this is. Component, Script, Tag or Layer?</term>
///         <term>The name of the thing</term>
///		    <term>A description of why this thing is needed</term>
///	    </item>
///	    <item>
///         <term>EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab)</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Alerted states</term>
///	    </item>
/// </list>
public class AlertedState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager;
    private IEnumerator _alertedCoroutine;
    private bool _alertedCoroutineIsRunning;

    void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
    }
    /// <summary>
    /// Enter alerted state
    /// </summary>
    public void Enter_Alerted()
    {
        //If the enemy is alerted by sound, it will look around for a few seconds
        if (_stateManager.alertedBySound)
        {
            //Stops the enemy from moving
            _stateManager.navMeshAgent.isStopped = true;
            _alertedCoroutineIsRunning = true;
            _alertedCoroutine = _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.stopWhenAlertedTime, () =>
            {
                //If the enemy stood still for the set time, it will continue
                _stateManager.navMeshAgent.isStopped = false; 
                _alertedCoroutineIsRunning = false;
            });
            StartCoroutine(_alertedCoroutine);
        }
    }
    
    /// <summary>
    /// Update alerted state
    /// </summary>
    public void Update_Alerted()
    {
        //stop if the coroutine is already running.
        if (_alertedCoroutineIsRunning) return;
        //If the enemy is alerted by sound it will investigate the sound.
        if (_stateManager.alertedBySound)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        //If the enemy is alerted by vision it will start chasing.
        else if (_stateManager.alertedByVision)
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }
        //If the enemy is not alerted at all anymore it will go back to patrolling.
        else
        {
            CustomEvent.Trigger(gameObject, "Patrol");
        }
    }
    
    /// <summary>
    /// Fixed update alerted state
    /// </summary>
    public void FixedUpdate_Alerted()
    {
        //The player looks around while it is alerted.
        _stateManager.LookAround();
    }
    
    /// <summary>
    /// Exit alerted state
    /// </summary>
    public void Exit_Alerted()
    {
        //Stop the coroutine if it is running.
        if(_alertedCoroutineIsRunning)StopCoroutine(_alertedCoroutine);
        _alertedCoroutineIsRunning = false;
    }
}
