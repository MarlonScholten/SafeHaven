using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NPC;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


/// <summary>
/// Unity event for when the player sound is detected
/// </summary>
[System.Serializable]
public class HeardASoundEvent : UnityEvent<SoundSource>
{
}

/// <summary>
/// Enemy AI that uses a state machine to control its behavior.
/// </summary>
public class EnemyAiStateManager : MonoBehaviour
{
    public List<Transform> wayPoints;
    public FSM_Scriptable_Object enemyAiScriptableObject;
    [NonSerialized] public NavMeshAgent navMeshAgent;
    [NonSerialized] public Vector3 targetWpLocation;
    [NonSerialized] public int currentWpIndex;
    [NonSerialized] public bool alertedBySound;
    [NonSerialized] public bool alertedByVision;
    [NonSerialized] public Vector3 spottedPlayerLastPosition;
    [NonSerialized] public GameObject spottedPlayer;
    [NonSerialized] public bool waitingAtWaypoint;
    [NonSerialized] public Vector3 locationOfNoise;
    [NonSerialized] public float timePlayerLastSpotted;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// This method rotates the enemy to a random direction.
    /// </summary>
    public void LookAround()
    {
        var x = Random.rotation;
        var z = Random.rotation;
        var position = transform.position;
        var randomRotation = new Vector3(x.x, Random.Range(position.y - 50, position.y + 50), z.z);
        var lookRotation = Quaternion.LookRotation((randomRotation- position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);

        // TODO: Play look around animation
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
        // TODO: Play walk animation
    }
    
    
    /// <summary>
    /// This method checks if the player is in the vision of the enemy.
    /// </summary>
    public bool CheckVision()
    {
        var foundObjects = Physics.OverlapSphere(transform.position, enemyAiScriptableObject.visionRange);
        var player = GetPlayer(foundObjects);
        if (player == null) return false;
        
        var transform1 = transform;
        var directionToPlayer = player.transform.position - transform1.position;
        var angleToPlayer = Vector3.Angle(transform1.forward, directionToPlayer);
        
        if (angleToPlayer >= enemyAiScriptableObject.visionAngle) return false;
        if (!Physics.Raycast(transform.position, directionToPlayer, out var hit, enemyAiScriptableObject.visionRange)) return false;
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(hit.transform.position, path);
        if (path.status == NavMeshPathStatus.PathPartial) return false;
        if (!hit.collider.gameObject.CompareTag("Sibling")) return false;
        
        var hitPlayer = hit.collider.gameObject;
        spottedPlayer = hitPlayer;
        spottedPlayerLastPosition = hitPlayer.transform.position;
        alertedByVision = true;
        alertedBySound = false;
        timePlayerLastSpotted = Time.time;
        return true;
    }
    
    /// <summary>
    /// This method check if the player is in the list of colliders.
    /// <param name="objects">Collided objects.</param>
    /// <returns>The player collider if exists.</returns>
    /// </summary>
    private static Collider GetPlayer(IEnumerable<Collider> objects)
    {
        return objects.FirstOrDefault(obj => obj.gameObject.CompareTag("Sibling"));
    }
    
    /// <summary>
    /// This method calculates the location where the enemy will investigate.
    /// The NavMeshSamplePosition method is used to find a closest point on the NavMesh to the location of the noise.
    /// </summary>
    public void CalculateInvestigateLocation(Vector3 position) {
        var randDirection = Random.insideUnitSphere * enemyAiScriptableObject.investigateDistance;
        randDirection += position;
        NavMesh.SamplePosition (randDirection, out NavMeshHit navHit, enemyAiScriptableObject.investigateDistance, 1);
        targetWpLocation = navHit.position;
        CheckPlayerPositionReachable(targetWpLocation);
        waitingAtWaypoint = false;
    }
    /// <summary>
    /// This method checks if the enemy is at the waypoint.
    /// </summary>
    public bool CheckIfEnemyIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, targetWpLocation) <= 2f;
    }

    /// <summary>
    /// This method checks if the player can reach the player position.
    /// </summary>
    public void CheckPlayerPositionReachable(Vector3 playerPosition)
    {
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(playerPosition, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            targetWpLocation = path.corners.Last();
            navMeshAgent.SetDestination(path.corners.Last());
            spottedPlayerLastPosition = path.corners.Last();
        }
        else
        {
            navMeshAgent.SetDestination(playerPosition);
        }
    }

    ///<summary>
    /// This method catches the child (reloads the scene for now.
    ///</summary>
    public static void CatchChild()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
