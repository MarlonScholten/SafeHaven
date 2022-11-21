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
        if(!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + 3) < Time.time)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        //get distance between player and enemy
        float distance = Vector3.Distance(_stateManager.spottedPlayer.transform.position, transform.position);
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
        if (_stateManager.CheckVision())
        {
            _stateManager.navMeshAgent.SetDestination(_stateManager.spottedPlayer.transform.position);
        }

        if (!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + 3) > Time.time)
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
        _stateManager.spottedPlayer = null;
    }
}
