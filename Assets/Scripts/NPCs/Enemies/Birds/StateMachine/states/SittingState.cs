using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Bird
{
    /// <summary>
    /// Author: Marlon Kerstens<br/>
    /// Modified by: N/A<br/>
    /// Description: This script is a the Sitting state.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>BirdObject</term>
    ///		    <term>Component</term>
    ///         <term>BirdObject</term>
    ///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Sitting states</term>
    ///	    </item>
    ///	    <item>
    ///         <term>BirdObject</term>
    ///		    <term>Script</term>
    ///         <term>BirdStateManager (Assets/Scripts/NPCs/Enemies/Birds/StateMachine/stateMachines/BirdStateManager.cs)</term>
    ///		    <term>This script contains variables that are used in this script to manage the state</term>
    ///	    </item>
    ///	    <item>
    ///         <term>BirdObject</term>
    ///		    <term>Visual scripting</term>
    ///         <term>BirdVisualScripting (Assets/Scripts/NPCs/Enemies/Birds/StateMachine/visualScripting/BirdVisualScripting.asset)</term>
    ///		    <term>This script need to be added to the BirdObject with the BirdVisualScripting</term>
    ///	    </item>
    /// </list>
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(BirdStateManager))]
    public class SittingState : MonoBehaviour
    {
        /// <summary>
        /// This is the BirdStateManager script that is used to manage the state
        /// </summary>
        private BirdStateManager _birdStateManager;

        /// <summary>
        /// This is the coroutine that is used to wait for a certain amount of time while sitting
        /// </summary>
        private IEnumerator _sittingCoroutine;

        /// <summary>
        /// Check if the _sittingCoroutine is running
        /// </summary>
        private bool _sittingCoroutineIsRunning;

        /// <summary>
        /// The awake method is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {
            _birdStateManager = GetComponent<BirdStateManager>();
        }

        /// <summary>
        /// This method is called when the state is entered
        /// </summary>
        public void Enter_Sitting_State()
        {
            // get rotation of _birdStateManager.restPoint

            transform.rotation = _birdStateManager.restPoint.rotation;
            _sittingCoroutineIsRunning = true;
            _sittingCoroutine = _birdStateManager.CallFunctionAfterSeconds(
                _birdStateManager.birdScriptableObject.TimeAtRestPoint,
                () => _sittingCoroutineIsRunning = false);
            StartCoroutine(_sittingCoroutine);
        }

        /// <summary>
        /// This method is called when the state is updated
        /// </summary>
        public void Update_Sitting_State()
        {
            if (!_sittingCoroutineIsRunning)
            {
                CustomEvent.Trigger(gameObject,
                    _birdStateManager.CheckIfAlertingObjectsAreNearby(_birdStateManager.birdScriptableObject.AlertTags)
                        ? "FlyingTowardsRestPoint"
                        : "FlyingTowardsNavmesh");
            }
        }

        /// <summary>
        /// This method is called when the state is fixed updated
        /// </summary>
        public void Fixed_Update_Sitting_State()
        {
            // not implemented yet
        }

        /// <summary>
        /// This method is called when the state is exited
        /// </summary>
        public void Exit_Sitting_State()
        {
            if (_sittingCoroutineIsRunning) StopCoroutine(_sittingCoroutine);
            _sittingCoroutineIsRunning = false;
        }
    }
}
