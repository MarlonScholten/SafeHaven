using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: <br/>
/// Description: Unity event for when an enemy is an guard and is alerted.
/// </summary>
[System.Serializable]
public class AlertEnemyEvent : UnityEvent<Vector3>
{
}

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: Hugo Ulfman<br/>
/// Description: This script is a the Alerted state of the enemy.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Alerted states</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Script</term>
///         <term>EnemyAIStateManager (Assets/Scripts/NPCs/Enemies/Behavior-AI/State machine/stateMachines/EnemyAiStateManager.cs)</term>
///		    <term>This script contains variables that are used in this script to manage the state</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Visual scripting</term>
///         <term>Enemy_Visual_Scripting (Assets/Scripts/NPCs/Enemies/Behavior-AI/State machine/visualScripting/Enemy_Visual_Scripting.asset)</term>
///		    <term>This script need to be added to the EnemyObject with the Enemy_Visual_Scripting</term>
///	    </item>
/// </list>
public class AlertedState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager; // Reference to the state manager
    private IEnumerator _alertedCoroutine; // a coroutine that is used to wait for a certain amount of time
    private bool _alertedCoroutineIsRunning; // a bool that is used to check if the coroutine is running

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        if (_stateManager.alertedBySound || _stateManager.isGuard)
        {
            //Stops the enemy from moving
            _stateManager.navMeshAgent.isStopped = true;
            _alertedCoroutineIsRunning = true;
            _alertedCoroutine = _stateManager.CallFunctionAfterSeconds(_stateManager.enemyAiScriptableObject.StopWhenAlertedTime, () =>
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
        if (_stateManager.isGuard)
        {
            AlertOtherEnemies();
            CustomEvent.Trigger(gameObject, "Patrol");
        }
        //If the enemy is alerted by sound it will investigate the sound.
        else if (_stateManager.alertedBySound || _stateManager.alertedByGuard &&  !_stateManager.isGuard)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
            //If the enemy is alerted by vision it will start chasing.
        else if (_stateManager.alertedByVision && !_stateManager.isGuard)
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
        //The Enemy looks around while it is alerted.
        if(!_stateManager.isGuard)_stateManager.LookAround();
        if (_stateManager.isGuard) LookTowardsPlayer();
   
    }

    private void LookTowardsPlayer()
    {
        // check if spottedPlayer is in vision with a raycast
        if(_stateManager.alertedBySound) _stateManager.RotateTowards(_stateManager.locationOfNoise);
        else if (_stateManager.alertedByVision) _stateManager.RotateTowards(_stateManager.spottedPlayer.transform.position);
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

    /// <summary>
    /// Alert other enemies due the AlertEnemyEvent.
    /// </summary>
    private void AlertOtherEnemies()
    {
        var ownPosition = transform.position;
        var enemiesInRadius = Physics.OverlapSphere(ownPosition, _stateManager.enemyAiScriptableObject.GuardAlertRadius)
            .Where(foundEnemy => foundEnemy.CompareTag("Enemy") && !foundEnemy.GetComponent<EnemyAiStateManager>().isGuard).ToArray();
        var givenPosition = ownPosition;
        if (_stateManager.alertedBySound) givenPosition = _stateManager.locationOfNoise;
        else if (_stateManager.alertedByVision) givenPosition = _stateManager.spottedPlayerLastPosition;
        foreach (var enemy in enemiesInRadius)
        {
            enemy.GetComponent<PatrolState>().AlertEnemyEvent.Invoke(givenPosition);
        }
    }
}
