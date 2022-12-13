using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace NPCs.Enemies.BirdsFlok
{
    /// <summary>
    /// Author: Marlon Kerstens<br/>
    /// Modified by: N/A<br/>
    /// Description: This script is used to follow a path from PathCreator.
    /// It CANNOT be used with the StateMachine Bird.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>BirdFlockObject (Assets/Prefabs/NPCs/Enemies/Bird/BirdFlockObject.prefab)</term>
    ///		    <term>Component</term>
    ///         <term>BirdFlockObject</term>
    ///		    <term>This script can be attached to the BirdFlockObject</term>
    ///	    </item>
    /// </list>
    public class PathFollower : MonoBehaviour
    {
        /// <summary>
        /// The path to follow.
        /// </summary>
        [SerializeField] private PathCreator pathToFollow;
        /// <summary>
        /// The speed of the bird.
        /// </summary>
        [SerializeField] private float speed = 5;
        /// <summary>
        /// The start point on the path in percentage.
        /// </summary>
        [SerializeField] private int startPointInPercentage = 0;
        /// <summary>
        /// The distance that the bird has traveled.
        /// </summary>
        private float _distanceTravelled;
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
           SetStartPoint();
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>

        void Update()
        {
            TravelPath();
        }

        /// <summary>
        /// This method is used to set the start point of the bird.
        /// </summary>
        private void SetStartPoint()
        {
            var pathLength = pathToFollow.path.length;
            _distanceTravelled = pathLength * startPointInPercentage / 100;
        }
        /// <summary>
        /// This method is used to travel the path.
        /// </summary>
        private void TravelPath()
        {
            _distanceTravelled += speed * Time.deltaTime;
            transform.position = pathToFollow.path.GetPointAtDistance(_distanceTravelled);
            transform.rotation = pathToFollow.path.GetRotationAtDistance(_distanceTravelled);
        }
    }
}
