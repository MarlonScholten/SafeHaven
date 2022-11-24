using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// <br>Author: Marlon Kerstens</br>
/// <br>Modified by: Hugo Ulfman</br>
/// Description: This script is a the Chasing state of the enemy.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab)</term>
///		    <term>Component</term>
///         <term>EnemyObject</term>
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Chasing states</term>
///	    </item>
///     <item>
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
public class ChasingState : MonoBehaviour
{
    private EnemyAiStateManager _stateManager; // Reference to the state manager
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
    }
    /// <summary>
    /// Enter chasing state
    /// </summary>
    public void Enter_Chasing()
    {
    }
    
    /// <summary>
    /// Update chasing state
    /// </summary>
    public void Update_Chasing()
    {
        //If the player/brother is not in vision and has not been for 3 seconds, the enemy goes back to investigate.
        if(!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + _stateManager.enemyAiScriptableObject.chaseTimeWhenNotSeen) < Time.time)
        {
            _stateManager.spottedPlayerLastPosition = _stateManager.spottedPlayer.transform.position;
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        //get distance between player/brother and enemy
        float distance = Vector3.Distance(_stateManager.spottedPlayer.transform.position, transform.position);
        //If the distance between the player/brother and enemy is less than the set distance, the enemy catches the player/brother.
        Debug.Log(distance + _stateManager.enemyAiScriptableObject.catchDistance);
        if (distance < _stateManager.enemyAiScriptableObject.catchDistance)
        {
            EnemyAiStateManager.CatchChild();
        }
    }
    
    /// <summary>
    /// Fixed update chasing state
    /// </summary>
    public void FixedUpdate_Chasing()
    {
        //If the player/brother is in vision the enemy moves towards the player/brother. If the player/brother is not in vision but has been in vision in the last 3 seconds, the enemy moves towards the player/brother.
        if (_stateManager.CheckVision() || (!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + _stateManager.enemyAiScriptableObject.chaseTimeWhenNotSeen) > Time.time))
        {
            _stateManager.CheckPlayerPositionReachable(_stateManager.spottedPlayer.transform.position);
        }
    }
    
    /// <summary>
    /// Exit chasing state
    /// </summary>
    public void Exit_Chasing()
    {
        //Reset spotted player/brother.
    }
}
