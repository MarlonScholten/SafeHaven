using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// This script is a the Chasing state of the enemy.
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
    private EnemyAiStateManager _stateManager;

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
        //If the player is not in vision and has not been for 3 seconds, the enemy goes back to investigate.
        if(!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + _stateManager.enemyAiScriptableObject.chaseTimeWhenNotSeen) < Time.time)
        {
            _stateManager.spottedPlayerLastPosition = _stateManager.spottedPlayer.transform.position;
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        //get distance between player and enemy
        float distance = Vector3.Distance(_stateManager.spottedPlayer.transform.position, transform.position);
        //If the distance between the player and enemy is less than the set distance, the enemy catches the player.
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
        //If the player is in vision the enemy moves towards the player. If the player is not in vision but has been in vision in the last 3 seconds, the enemy moves towards the player.
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
        //Reset spotted player.
    }
}
