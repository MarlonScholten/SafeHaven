using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: Hugo Ulfman, Thomas van den Oever<br/>
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
    private SoundManager.EnemyStateWatcher _enemyStateWatcher;

    ChasingVignette chasingVignette;
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _stateManager = GetComponent<EnemyAiStateManager>();
        _enemyStateWatcher = GameObject.Find("EnemyStateWatcher").GetComponent<SoundManager.EnemyStateWatcher>();
    }

    void Start() {
        chasingVignette = _stateManager._postProcessing.GetComponent<ChasingVignette>();
    }

    /// <summary>
    /// Enter chasing state
    /// </summary>
    public void Enter_Chasing()
    {
        chasingVignette.Increase();
        _stateManager.navMeshAgent.speed = _stateManager.enemyAiScriptableObject.ChaseSpeed;
        //shows the current state as text above the enemy when this is enabled in the inspector.
        if (_stateManager.enemyAiScriptableObject.showCurrentState)
        {
            _stateManager.textMesh.text = "Chasing";
            _stateManager.textMesh.color = Color.red;
        }

        GameObject.Find("EnemyStateWatcher").GetComponent<SoundManager.EnemyStateWatcher>().IsChasing(true);
    }
    
    /// <summary>
    /// Update chasing state
    /// </summary>
    public void Update_Chasing()
    {
        //If the player/brother is not in vision and has not been for 3 seconds, the enemy goes back to investigate.
        if(!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + _stateManager.enemyAiScriptableObject.ChaseTimeWhenNotSeen) < Time.time)
        {
            _stateManager.spottedPlayerLastPosition = _stateManager.spottedPlayer.transform.position;
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        _stateManager.CheckForCatching(_stateManager.spottedPlayer);
    }



    /// <summary>
    /// Fixed update chasing state
    /// </summary>
    public void FixedUpdate_Chasing()
    {
        //If the player/brother is in vision the enemy moves towards the player/brother. If the player/brother is not in vision but has been in vision in the last 3 seconds, the enemy moves towards the player/brother.
        if (_stateManager.CheckVision() || (!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + _stateManager.enemyAiScriptableObject.ChaseTimeWhenNotSeen) > Time.time))
        {
            _stateManager.CheckPositionReachable(_stateManager.spottedPlayer.transform.position);
        }
    }
    
    /// <summary>
    /// Exit chasing state
    /// </summary>
    public void Exit_Chasing()
    {
        chasingVignette.Decrease();
        //Reset spotted player/brother.
        _stateManager.navMeshAgent.speed = _stateManager.defaultSpeed;
        _enemyStateWatcher.IsChasing(false);
        _stateManager.isChasing = false;
    }
}
