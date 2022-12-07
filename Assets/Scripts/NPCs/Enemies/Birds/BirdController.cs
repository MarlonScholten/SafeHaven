// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using NPCs.Enemies.Birds;
// using PathCreation;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UIElements;
// using Random = UnityEngine.Random;
//
// public class BirdController : MonoBehaviour
// {
//
//     private NavMeshAgent _navMeshAgent;
//     private Vector3 startPoint;
//     private Vector3 targetWpLocation;
//     private bool isWaiting;
//     public bool alerted;
//     private bool _isOnPathUp;
//     private bool _isOnPathDown;
//     private float _distanceTravelled;
//     private bool _waitingCoroutine;
//     private EndOfPathInstruction _endOfPathInstruction = EndOfPathInstruction.Stop;
//     [SerializeField] private PathCreator path;
//     [SerializeField] private float speed;
//     [SerializeField] private float secondsToWaitBetweenUpDown = 2f;
//     private PathCreator _createdPath;
//     private bool travelHeadline;
//     private void Start()
//     {
//        
//         // CalculateNextWalkableWaypoint(startPoint);
//     }
//
//     public void FixedUpdate()
//     {
//         // if(!alerted && !travelHeadline && !_isOnPathDown) Walking();
//         // if (alerted)
//         // {   _navMeshAgent.enabled = false;
//         //     if (_createdPath == null)
//         //     {
//         //         _createdPath = CreatePathToClosestPointOnGivenPath(path);
//         //     }
//         //     TravelPath(_createdPath);
//         //     // check if end of path is reached
//         //     if (_distanceTravelled >= _createdPath.path.length)
//         //     {
//         //         var closestPointOnPath = path.path.GetClosestPointOnPath(transform.position);
//         //         // set distance travelled to start of path plus distance to closest point on path
//         //         _distanceTravelled = path.path.GetClosestDistanceAlongPath(transform.position) + closestPointOnPath.magnitude;
//         //         
//         //         travelHeadline = true;
//         //         alerted = false;
//         //     }
//         // }
//         // if (travelHeadline) TravelPath(path);
//         //
//         
//         
//         
//         // if (alerted && !_isOnPathUp)
//         // {
//         //     _navMeshAgent.SetDestination(pathUp.path.GetPoint(0));
//         //     if (Vector3.Distance(transform.position, pathUp.path.GetPoint(0)) < 1f)
//         //     {
//         //         _isOnPathUp = true;
//         //         _navMeshAgent.enabled = false;
//         //     }
//         // }
//         // if(alerted && _isOnPathUp){
//         //     TravelPath(pathUp);
//         //     if (_distanceTravelled >= pathUp.path.length && !_waitingCoroutine)
//         //     {
//         //         _waitingCoroutine = true;
//         //         StartCoroutine(CallFunctionAfterSeconds(secondsToWaitBetweenUpDown, () =>
//         //         {
//         //             _distanceTravelled = 0;
//         //             alerted = false; 
//         //             _isOnPathUp = false;
//         //             _isOnPathDown = true;
//         //             _waitingCoroutine = false;
//         //         }));
//         //     }
//         // }
//         // if(!alerted && _isOnPathDown){
//         //     TravelPath(pathDown);
//         //     if (!(_distanceTravelled >= pathDown.path.length)) return;
//         //     DetachToNavmesh();
//         //     _distanceTravelled = 0;
//         // }
//     }
//
//  
//
//     private void AttachToNavmesh()
//     {
//         _navMeshAgent.enabled = true;
//         if (!NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas)) return;
//         _navMeshAgent.SetDestination(hit.position);
//         targetWpLocation = hit.position;
//         _isOnPathDown = false;
//     }
//
//     private void TravelPath(PathCreator path){
//         _distanceTravelled += speed * Time.deltaTime;
//         transform.position = path.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
//         transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
//     }
//     
//     IEnumerator CallFunctionAfterSeconds(float seconds, Action function)
//     {
//         yield return new WaitForSeconds(seconds);
//         function();
//     }
//     
//     
//     // _navMeshAgent.enabled = true;
//     // NavMeshHit hit;
//     //     if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
//     // { 
//     //     _navMeshAgent.SetDestination(hit.position);
//     //     xxx = false;
//     //     _isOnPathUp = false;
//     // }
//     
//
//
//
//     
//     /// <summary>
//     /// This method checks if the enemy can reach the player/brother position.
//     /// </summary>
//     public void CheckPlayerPositionReachable(Vector3 playerPosition)
//     {
//     }
//
// }