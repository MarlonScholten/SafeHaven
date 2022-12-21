using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bird
{
    /// <summary>
    /// Author: Marlon Kerstens<br/>
    /// Modified by: N/A<br/>
    /// Description: This script is a the Walking state.
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
    ///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Walking states</term>
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
    public class WalkingState : MonoBehaviour
    {
        /// <summary>
        /// This is the BirdStateManager script that is used to manage the state
        /// </summary>
        private BirdStateManager _birdStateManager;

        /// <summary>
        /// This is the start position of the bird
        /// </summary>
        private Vector3 startPoint;

        /// <summary>
        /// This is the target position of the bird
        /// </summary>
        private Vector3 targetWpLocation;

        /// <summary>
        /// Check if bird is rotating
        /// </summary>
        private bool _isRotating;

        /// <summary>
        /// Coroutine for the rotation
        /// </summary>
        private IEnumerator _rotateCoroutine;

        /// <summary>
        /// Check of the _rotateCoroutine is running
        /// </summary>
        private bool _rotateCoroutineIsRunning;
        
        
        /// <summary>
        /// Coroutine for set that the bird can be alerted.
        /// </summary>
        private IEnumerator _alertCoroutine;

        /// <summary>
        /// Check if the alert coroutine is running.
        /// </summary>
        private bool _alertCoroutineIsRunning;

        /// <summary>
        /// Check if the bird can be alerted.
        /// </summary>
        private bool _canBeAlerted;

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
        public void Enter_Walking_State()
        {
            _birdStateManager.animator.SetInteger("state", 1);
            _alertCoroutine = _birdStateManager.CallFunctionAfterSeconds(_birdStateManager.birdScriptableObject.TimeBetweenLadingAndAlert, () => _canBeAlerted = true);
            StartCoroutine(_alertCoroutine);
            var position = transform.position;
            _birdStateManager.groundHeight = position.y;
            startPoint = position;
            CalculateNextWalkableWaypoint(startPoint);
        }

        /// <summary>
        /// This method is called when the state is updated
        /// </summary>
        public void Update_Walking_State()
        {
            if (_birdStateManager.CheckIfAlertingObjectsAreNearby(_birdStateManager.birdScriptableObject.AlertTags) && _canBeAlerted)
            {
                CustomEvent.Trigger(gameObject, "FlyingTowardsRestPoint");
            }
        }

        /// <summary>
        /// This method is called when the state is fixed updated
        /// </summary>
        public void Fixed_Update_Walking_State()
        {
            if (_birdStateManager.CheckIfIsAtWaypoint(targetWpLocation) && !_isRotating)
            {
                CalculateNextWalkableWaypoint(startPoint);
            }
        }

        /// <summary>
        /// This method is called when the state is exited
        /// </summary>
        public void Exit_Walking_State()
        {
            targetWpLocation = Vector3.zero;
            startPoint = Vector3.zero;
            _birdStateManager.groundHeight = transform.position.y;
            if (_rotateCoroutineIsRunning) StopCoroutine(_rotateCoroutine);
            _rotateCoroutineIsRunning = false;
            if(_alertCoroutineIsRunning) StopCoroutine(_alertCoroutine);
            _alertCoroutineIsRunning = false;
            _canBeAlerted = false;
        }

        /// <summary>
        /// This method is used to calculate the next waypoint
        /// <param name="position">The position where the next waypoint need to be calculated</param>
        /// </summary>
        private void CalculateNextWalkableWaypoint(Vector3 position)
        {
            var randDirection = Random.insideUnitSphere * _birdStateManager.birdScriptableObject.WalkRadius;
            randDirection += position;
            NavMesh.SamplePosition(randDirection, out var navHit, _birdStateManager.birdScriptableObject.WalkRadius, 1);
            var path = new NavMeshPath();
            _birdStateManager.navMeshAgent.CalculatePath(navHit.position, path);
            if (path.status == NavMeshPathStatus.PathPartial)
            {
                targetWpLocation = path.corners.Last();
                _birdStateManager.navMeshAgent.SetDestination(path.corners.Last());
            }
            else
            {
                targetWpLocation = navHit.position;
                _birdStateManager.navMeshAgent.SetDestination(navHit.position);

            }

            _rotateCoroutine = RotateToNextPosition();
            StartCoroutine(_rotateCoroutine);
        }

        /// <summary>
        /// This method is used to rotate the bird to the next waypoint
        /// <returns>IEnumerator</returns>
        /// </summary>
        private IEnumerator RotateToNextPosition()
        {
            _rotateCoroutineIsRunning = true;
            _birdStateManager.navMeshAgent.isStopped = true;
            _isRotating = true;
            var dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
            while (dot < 0.9f)
            {
                dot = Vector3.Dot(transform.forward, (targetWpLocation - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(targetWpLocation - transform.position),
                    _birdStateManager.birdScriptableObject.RotateSpeed * Time.deltaTime);
                yield return null;
            }

            _rotateCoroutineIsRunning = false;
            _birdStateManager.navMeshAgent.isStopped = false;
            _isRotating = false;
        }
    }
}