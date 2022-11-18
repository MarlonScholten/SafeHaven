using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChasingState : MonoBehaviour
{
    private Enemy_Finite_State_Machine _stateManager;
    void Start()
    {
        _stateManager = FindObjectOfType<Enemy_Finite_State_Machine>();
    }
    
    /// CHASING ///
    
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
        if(!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + 2) < Time.time)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
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

        if (!_stateManager.CheckVision() && (_stateManager.timePlayerLastSpotted + 2) > Time.time)
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
