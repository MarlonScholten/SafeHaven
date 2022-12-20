using System.Collections.Generic;
using System.Linq;
using PathCreation;
using PathCreation.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace Bird
{
    /// <summary>
    /// Author: Marlon Kerstens<br/>
    /// Modified by: N/A<br/>
    /// Description: This script is a the Flying Towards Rest point state.
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
    ///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Flying Towards Rest point states</term>
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
    public class FlyingTowardsRestpointState : MonoBehaviour
    {
        /// <summary>
        /// This is the BirdStateManager script that is used to manage the state
        /// </summary>
        private BirdStateManager _birdStateManager;

        /// <summary>
        /// The path that the bird will follow
        /// </summary>
        private PathCreator _path;

        /// <summary>
        /// The distance that the bird has traveled
        /// </summary>
        private float _distanceTravelled;

        /// <summary>
        /// Instructions that holds the EndOfPathInstruction
        /// </summary>
        private const EndOfPathInstruction EndOfPathInstruction = PathCreation.EndOfPathInstruction.Stop;

        /// <summary>
        /// The Awake method is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {
            _birdStateManager = GetComponent<BirdStateManager>();
        }

        /// <summary>
        /// This method is called when the state is entered
        /// </summary>
        public void Enter_Flying_Towards_Rest_Point_State()
        {

            DetachFromNavmesh();
            _birdStateManager.restPoint = GetClosestRestPoint();
            _path = _birdStateManager.CreatePathToClosestPointOnGivenPath(_birdStateManager.restPoint.transform.position);
        }

        /// <summary>
        /// This method is called when the state is updated
        /// </summary>
        public void Update_Flying_Towards_Rest_Point_State()
        {
            if (transform.position == _birdStateManager.restPoint.transform.position)
            {
                CustomEvent.Trigger(gameObject, "Sitting");
            }
        }

        /// <summary>
        /// This method is called when the state is fixed updated
        /// </summary>
        public void Fixed_Update_Flying_Towards_Rest_Point_State()
        {
            if (transform.position != _birdStateManager.restPoint.transform.position)
            {
                TravelPath(_path);
            }
        }

        /// <summary>
        /// This method is called when the state is exited
        /// </summary>
        public void Exit_Flying_Towards_Rest_Point_State()
        {
            _path = null;
            Destroy(_birdStateManager.pathGameObject);
            _distanceTravelled = 0;
        }

        /// <summary>
        /// This method checks which rest point is the closest to the bird
        /// <returns>Transform</returns>
        /// </summary>
        private GameObject GetClosestRestPoint()
        {
            var restPoints = GameObject.FindGameObjectsWithTag("BirdRestPoint");
            GameObject closest = null;
            var distance = Mathf.Infinity;
            var position = transform.position; 
            foreach (var restPointObject in restPoints.Select(rp => rp))
            {
                if(restPointObject.GetComponent<BirdRestPointVariables>().isBirdOnRestPoint) continue;
                if (restPointObject.transform.position == transform.position) continue;
                if (_birdStateManager.lastRestPoints.Contains(restPointObject.transform.position)) continue;
                var diff = restPointObject.transform.position - position;
                var curDistance = diff.sqrMagnitude;
                if (curDistance >= distance) continue;
                closest = restPointObject;
                distance = curDistance;
            }
            if(closest == null)
            {
                _birdStateManager.lastRestPoints = new List<Vector3>();
                return GetClosestRestPoint();
            }

            closest.GetComponent<BirdRestPointVariables>().isBirdOnRestPoint = true;
            _birdStateManager.lastRestPoints.Add(closest.transform.position);
            return closest;
        }

        /// <summary>
        /// This method makes the bird travel the path.
        /// <param name="path">The path that the bird will travel</param>
        /// </summary>
        private void TravelPath(PathCreator path)
        {
            _distanceTravelled += _birdStateManager.birdScriptableObject.FlySpeed * Time.deltaTime;
            transform.position = path.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction);
            transform.rotation = Quaternion.LookRotation(path.path.GetDirectionAtDistance(_distanceTravelled, EndOfPathInstruction), Vector3.up);
        }

        /// <summary>
        /// This method detaches the bird from the navmesh
        /// </summary>
        private void DetachFromNavmesh()
        {
            _birdStateManager.navMeshAgent.enabled = false;
        }
    }
}