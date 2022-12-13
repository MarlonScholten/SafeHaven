using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: N/A<br/>
/// Description: This script is a the Flying Towards NavmeshState state.
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
///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Flying Towards Navmesh states</term>
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
public class FlyingTowardsNavmeshState : MonoBehaviour
{
    /// <summary>
    /// This is the BirdStateManager script that is used to manage the state
    /// </summary>
    private BirdStateManager _birdStateManager;
    /// <summary>
    /// The destination on the navmesh
    /// </summary>
    private Vector3 _destinationAtNavmesh;
    /// <summary>
    /// The path to the destination
    /// </summary>
    private PathCreator _path;
    /// <summary>
    /// The distance that the bird has traveled
    /// </summary>
    private float _distanceTravelled;
    /// <summary>
    /// Instruction that holds the EndOfPathInstruction variable
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
    public void ENTER_FLYING_TOWARDS_NAVMESH_STATE()
    {
        _destinationAtNavmesh = GetPointOnNavmesh();
        _path = _birdStateManager.CreatePathToClosestPointOnGivenPath(_destinationAtNavmesh);
    }
    /// <summary>
    /// This method is called when the state is updated
    /// </summary>
    public void UPDATE_FLYING_TOWARDS_NAVMESH_STATE()
    {
        // if the bird is close enough to the destination, then switch to the next state
        if (_birdStateManager.CheckIfIsAtWaypoint(_destinationAtNavmesh))
        {
            AttachToNavmesh();
            CustomEvent.Trigger(gameObject, "Walking");
        }
    }
    /// <summary>
    /// This method is called when de state is fixed updated
    /// </summary>
    public void FIXED_UPDATE_FLYING_TOWARDS_NAVMESH_STATE()
    {
        TravelPath(_path);
    }
    /// <summary>
    /// This method is called when the state is exited
    /// </summary>
    public void EXIT_FLYING_TOWARDS_NAVMESH_STATE()
    {
        _path = null;
        Destroy(_birdStateManager.pathGameObject);
        _distanceTravelled = 0;
        _birdStateManager.restPoint = null;
        _birdStateManager.lastRestPoint = Vector3.zero;
    }
    /// <summary>
    /// This method is used to travel the bird along a path
    /// <param name="path">The path that te bird will travel</param>
    /// </summary>
    private void TravelPath(PathCreator path){
        _distanceTravelled += _birdStateManager.birdScriptableObject.FlySpeed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction);
    }
    /// <summary>
    /// This method is used to get a random point on the navmesh
    /// <returns>Random point in Vector3</returns>
    /// </summary>
    private Vector3 GetPointOnNavmesh()
    {
        var point = transform.position;
        point.y = 0;
        point.x += Random.Range(4, 6) * (Random.value > 0.5f ? 1 : -1);
        point.z += Random.Range(4, 6) * (Random.value > 0.5f ? 1 : -1);
        NavMesh.SamplePosition(point, out var hit, 100, NavMesh.AllAreas);
        return hit.position;
    }
    
    /// <summary>
    /// This method is used to attach the bird to the navmesh
    /// </summary>
    private void AttachToNavmesh()
    {
        _birdStateManager.navMeshAgent.enabled = true;
    }
}

