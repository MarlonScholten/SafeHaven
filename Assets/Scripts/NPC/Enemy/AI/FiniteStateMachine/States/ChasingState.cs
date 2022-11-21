using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ChasingState functions
/// </summary>
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
            _stateManager.catchChild();
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
            _stateManager.navMeshAgent.SetDestination(_stateManager.spottedPlayer.transform.position);
        }
    }
    
    /// <summary>
    /// Exit chasing state
    /// </summary>
    public void Exit_Chasing()
    {
        _stateManager.CheckPlayerPositionReachable(_stateManager.spottedPlayer.transform.position);
        //Reset spotted player.
        //_stateManager.spottedPlayer = null;
    }
}
