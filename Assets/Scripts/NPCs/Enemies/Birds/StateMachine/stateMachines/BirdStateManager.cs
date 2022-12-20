using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Bird
{
    /// <summary>
    /// <br>Author: Marlon Kerstens</br>
    /// <br>Modified by: N/A </br>
    /// Description: This script is used as a manager to control the variables en methods that are shared in different states on the BirdObject.
    /// How to use BIRD AI.
    /// 1. Add a NavmeshSurface to the scene,
    /// 2. Add the BirdRestPoint prefabs to the scene (Assets/Prefabs/NPCs/Enemies/Bird/BirdRestPoint.prefab),
    /// 3. Add the BirdObject prefabs to the scene (Assets/Prefabs/NPCs/Enemies/Bird/BirdObject.prefab),
    /// 4. Change the Tags that alerts the bird in de list of the BirdScriptableObject.
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
    ///         <term>NavmeshAgent</term>
    ///		    <term>This script contains actions for moving the enemy with the navmeshAgent.setDestination() function.</term>
    ///	    </item>
    ///	    <item>
    ///         <term>BirdObject</term>
    ///		    <term>Prefabs</term>
    ///         <term>BirdRestPoint (Assets/Prefabs/NPCs/Enemies/Bird/BirdRestPoint.prefab)</term>
    ///		    <term>This script needs BirdsRestPoints</term>
    ///	    </item>
    /// </list>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(WalkingState))]
    [RequireComponent(typeof(FlyingTowardsNavmeshState))]
    [RequireComponent(typeof(FlyingTowardsRestpointState))]
    [RequireComponent(typeof(InitializeStateBird))]
    [RequireComponent(typeof(SittingState))]
    [RequireComponent(typeof(StateMachine))]
    public class BirdStateManager : MonoBehaviour
    {
        /// <summary>
        /// The scriptable object variables that are used in this script.
        /// </summary>
        public BirdScriptableObject birdScriptableObject;

        /// <summary>
        /// The navmesh agent component.
        /// </summary>
        [NonSerialized] public NavMeshAgent navMeshAgent;

        /// <summary>
        /// The rest point that the bird is currently going to.
        /// </summary>
        [NonSerialized] public GameObject restPoint;

        /// <summary>
        /// The last rest point that the bird was at.
        /// </summary>
        [NonSerialized] public List<Vector3> lastRestPoints;

        /// <summary>
        /// The ground height where it can walk.
        /// </summary>
        [NonSerialized] public float groundHeight;

        /// <summary>
        /// Save the game object so it can be deleted from the scene
        /// </summary>
        [NonSerialized] public GameObject pathGameObject;
        
        /// <summary>
        /// The animator component.
        /// </summary>
        [NonSerialized] public Animator animator;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            lastRestPoints = new List<Vector3>();
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// This method calls a method after a certain amount of seconds.
        /// <param name="seconds">Amount of seconds that takes to call the method.</param>
        /// <param name="method">The method that need to be called after the amount of seconds.</param>
        /// </summary>   
        public IEnumerator CallFunctionAfterSeconds(int seconds, Action method)
        {
            yield return new WaitForSeconds(seconds);
            method();
        }

        /// <summary>
        /// This method creates a path for the bird to follow.
        /// <param name="destination">The destination for the path</param>
        /// <returns>A Path in a PathCreator instance</returns>
        /// </summary>
        public PathCreator CreatePathToClosestPointOnGivenPath(Vector3 destination)
        {
            var position = transform.position;
            pathGameObject = new GameObject();
            var createdPath = pathGameObject.AddComponent<PathCreator>();
            var startCurve = Vector3.Lerp(position, destination, 0.25f);
            startCurve.y = position.y;
            var endCurve = Vector3.Lerp(position, destination, 0.75f);
            endCurve.y = destination.y;

            if (destination.y >= position.y && destination.y <= position.y + 2)
            {
                startCurve.y = position.y + 2;
            }
            var bezierPath = new BezierPath(new List<Vector3> { position, startCurve, endCurve, destination }, false)
            {
                FlipNormals = position.x < destination.x,
            };
            createdPath.bezierPath = bezierPath;
            createdPath.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            return createdPath;
        }

        /// <summary>
        /// This method checks if the alerting object is near the bird.
        /// <param name="alertingObjects">A collection of Tags</param>
        /// <returns>True if the alerting object is near the bird.</returns>
        /// </summary>
        public bool CheckIfAlertingObjectsAreNearby(ICollection<string> alertingObjects)
        {
            var position = transform.position;
            position.y = groundHeight;
            var colliders = Physics.OverlapSphere(position, birdScriptableObject.AlertRadius);
            return colliders.Any(col => alertingObjects.Contains(col.gameObject.tag));
        }

        /// <summary>
        /// This method checks if the bird is at destination.
        /// <param name="destination">Vector3 in the world</param>
        /// <returns>True if the bird is at destination.</returns>
        /// </summary>
        public bool CheckIfIsAtWaypoint(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) <= 0.5f;
        }
    }
}