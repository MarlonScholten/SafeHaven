using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NPC;
using SoundManager;
using TMPro;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Author: Hugo Ulfman<br/>
/// Modified by: <br/>
/// Description: Unity event for when the player/brother sound is detected. This is created so a UnityEvent can pass an argument
/// </summary>
[System.Serializable]
public class HeardASoundEvent : UnityEvent<SoundSource>
{
}

/// <summary>
/// <br>Author: Marlon Kerstens</br>
/// <br>Modified by: Hugo Ulfman and Iris Giezen</br>
/// Description: This script is used as a manager to control the variables en methods that are shared in different states on the EnemyObject (Assets/Prefabs/NPCs/Enemies/EnemyObject.prefab).
/// How to use ENEMY AI:
/// 1. Add a NavmeshSurface to the scene,
/// 2. Add the EnemyWaypoint prefabs to the scene (Assets/Prefabs/NPCs/Enemies/EnemyWayPoint.prefab),
/// 3. Add the EnemyObject prefabs to the scene (Assets/Prefabs/NPCs/Enemies/EenmyObject.prefab),
/// 4. Determine the route of the enemy with adding the waypoints to the waypoint list of the EnemyObject.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Component</term>
///         <term>NavmeshAgent</term>
///		    <term>This script contains actions for moving the enemy with the navmeshAgent.setDestination() function.</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Tag</term>
///         <term>Player</term>
///		    <term>This script checks if an object with the "Player" tag is in the cone vision.</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Tag</term>
///         <term>Brother</term>
///		    <term>This script checks if an object with the "Brother" tag is in the cone vision.</term>
///	    </item>
///	    <item>
///         <term>EnemyObject</term>
///		    <term>Prefabs</term>
///         <term>EnemyWaypoint (Assets/Prefabs/NPCs/Enemies/EnemyWayPoint.prefab)</term>
///		    <term>This script needs EnemyWaypoints to patrol.</term>
///	    </item>
/// </list>
///
/// <summary>
/// This requireComponent is used for the triggers. The AkComponents needs that the object to trigger the trigger has an rigidbody. But make it kineMatic so it does not affect anything.
/// </summary>
public class EnemyAiStateManager : MonoBehaviour
{
    [Tooltip("Scriptable object that contains the adjustable variables for the enemy")]
    public FSM_Scriptable_Object enemyAiScriptableObject;

    [Tooltip("Boolean to set if the enemy is an guard or not")]
    public bool isGuard; // if the enemy is a guard or not
    
    [HideInInspector] public List<GameObject> wayPoints; // List of waypoints if not a guard
    [HideInInspector] public GameObject guardWaypoint; // Waypoint if guard
    [NonSerialized] public NavMeshAgent navMeshAgent; // Navmesh agent component
    [NonSerialized] public float defaultSpeed; // The speed of the enemy that is set in the navmesh agent
    [NonSerialized] public Vector3 targetWpLocation; // Location of the current target waypoint
    [NonSerialized] public int currentWpIndex; // Index of the current target waypoint
    [NonSerialized] public bool alertedBySound; // Boolean to check if the enemy is alerted by a sound
    [NonSerialized] public bool alertedByVision; // Boolean to check if the enemy is alerted by vision
    [NonSerialized] public bool alertedByGuard; // Boolean to check if the enemy is alerted by a guard

    [NonSerialized] public Vector3 spottedPlayerLastPosition; // Last position of the player/brother when the enemy spotted him
    [NonSerialized] public Vector3 recievedLocationFromGuard; // Last position of the player/brother when the enemy spotted him

    [NonSerialized] public GameObject spottedPlayer; // The player/brother that the enemy spotted
    [NonSerialized] public bool waitingAtWaypoint; // Boolean to check if the enemy is waiting at a waypoint
    [NonSerialized] public Vector3 locationOfNoise; // Location of the noise that the enemy heard
    [NonSerialized] public float timePlayerLastSpotted; // Time when the enemy last spotted the player/brother
    
    [NonSerialized] public TextMeshPro textMesh; // TextMesh component for state visualization
    
    [NonSerialized] public Animator animator; // Animator component
    private static readonly int s_forwardVelocity = Animator.StringToHash("forwardVelocity"); // Animator parameter for forward velocity
    
    private EnemyStateWatcher _enemyStateWatcher; // EnemyStateWatcher component

    [NonSerialized]
    public GameObject _postProcessing;

    


    private void Awake()
    {
        _postProcessing = GameObject.Find("PostProcessing");
        //Fetches if the option to show the state of the enemy is selected and makes it empty if not used.
        textMesh = GetComponentInChildren<TextMeshPro>();
        if (!enemyAiScriptableObject.showCurrentState)
        {
            textMesh.text = "";
        }

        _enemyStateWatcher = FindObjectOfType<EnemyStateWatcher>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        defaultSpeed = navMeshAgent.speed;
    }

    private void Update()
    {
        if (enemyAiScriptableObject.showCurrentState)
        {
            textMesh.transform.rotation = Camera.main.transform.rotation;
        }

        
        
    }

    private void FixedUpdate()
    {
        animator.SetFloat(s_forwardVelocity, navMeshAgent.velocity.magnitude);
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
        var lookRotation = Quaternion.LookRotation((randomRotation - position).normalized);
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
    /// This method checks if the player/brother is in the vision of the enemy.
    /// </summary>
    public bool CheckVision()
    {
        var foundObjects = Physics.OverlapSphere(transform.position, enemyAiScriptableObject.VisionRange);
        var playerOrBrother = GetPlayerOrBrotherOutOfColliderList(foundObjects);
        if (CheckIfIsInImmediateChaseRadius(playerOrBrother)) return true;
        if (playerOrBrother == null) return false;
        var transform1 = transform;
        var directionToPlayer = playerOrBrother.transform.position - transform1.position;
        var angleToPlayer = Vector3.Angle(transform1.forward, directionToPlayer);
        if (angleToPlayer >= enemyAiScriptableObject.VisionAngle)
            return false; // Check if the player is in set vision angle
        if (!Physics.Raycast(transform.position + new Vector3(0f, transform.lossyScale.y / 2, 0f), directionToPlayer,
                out var hit, enemyAiScriptableObject.VisionRange))
            return false; // Check if the player is in set vision range
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(hit.transform.position, path);
        if (path.status == NavMeshPathStatus.PathPartial) return false; // Check if the player is reachable
        if (!hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("Brother"))
            return false; // Check if the object is the player/brother

        var hitPlayer = hit.collider.gameObject;
        spottedPlayer = hitPlayer;
        spottedPlayerLastPosition = hitPlayer.transform.position;
        alertedByVision = true;
        alertedBySound = false;
        alertedByGuard = false;
        timePlayerLastSpotted = Time.time;
        return true;
    }

    private bool CheckIfIsInImmediateChaseRadius(Collider player)
    {
        if (player == null || Vector3.Distance(transform.position, player.transform.position) >= enemyAiScriptableObject.ImmediateChaseRadius) return false;
        if (!Physics.Raycast(transform.position + new Vector3(0f, transform.lossyScale.y / 2, 0f),
                player.transform.position - transform.position, out var hit1,
                enemyAiScriptableObject.ImmediateChaseRadius)) return false;
        if (!hit1.collider.CompareTag("Player") && !hit1.collider.CompareTag("Brother")) return false;
        spottedPlayer = hit1.collider.gameObject;
        spottedPlayerLastPosition = hit1.collider.transform.position;
        alertedByVision = true;
        alertedBySound = false;
        alertedByGuard = false;
        timePlayerLastSpotted = Time.time;
        return true;
    }

    /// <summary>
    /// This method check if the player/brother is in the list of colliders.
    /// <param name="objects">Collided objects.</param>
    /// <returns>The player/brother collider if exists.</returns>
    /// </summary>
    private static Collider GetPlayerOrBrotherOutOfColliderList(IEnumerable<Collider> objects)
    {
        return objects.FirstOrDefault(
            obj => obj.gameObject.CompareTag("Player") || obj.gameObject.CompareTag("Brother"));
    }

    /// <summary>
    /// This method calculates the location where the enemy will investigate.
    /// The NavMeshSamplePosition method is used to find a closest point on the NavMesh to the location of the noise.
    /// </summary>
    public void CalculateInvestigateLocation(Vector3 position)
    {
        var randDirection = Random.insideUnitSphere * enemyAiScriptableObject.InvestigateDistance;
        randDirection += position;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, enemyAiScriptableObject.InvestigateDistance, 1);
        targetWpLocation = !navHit.hit ? position : navHit.position;
        CheckPositionReachable(targetWpLocation);
        waitingAtWaypoint = false;
    }

    /// <summary>
    /// This method checks if the enemy is at the waypoint.
    /// </summary>
    public bool CheckIfEnemyIsAtWaypoint()
    {
        return Vector3.Distance(transform.position, targetWpLocation) <= 1f;
    }

    /// <summary>
    /// This method checks if the enemy can reach the player/brother position.
    /// </summary>
    public void CheckPositionReachable(Vector3 position)
    {
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(position, path);
        if (path.status is NavMeshPathStatus.PathPartial)
        {
            targetWpLocation = path.corners.Last();
            navMeshAgent.SetDestination(path.corners.Last());
            if (alertedByVision) spottedPlayerLastPosition = path.corners.Last();
            if (alertedByGuard) recievedLocationFromGuard = path.corners.Last();
        }
        else
        {
            targetWpLocation = position;
            navMeshAgent.SetDestination(position);
        }
    }

    ///<summary>
    /// This method catches the child, and invokes the corresponding <see cref="EnemyStateWatcher.OnSisterCaught"/> and <see cref="EnemyStateWatcher.OnBrotherCaught"/> events with the help of a relay.
    ///</summary>
    public void CatchChild(GameObject child)
    {
        // Invoke the correct caught event.
        if (child.CompareTag("Brother"))
            _enemyStateWatcher.BrotherCaught();
        else if (child.CompareTag("Player"))
            _enemyStateWatcher.SisterCaught();
    }

    /// <summary>
    /// This method rotates the enemy towards a target.
    /// </summary>
    /// <param name="target">Target to rotate towards</param>
    public void RotateTowards(Vector3 target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(target - transform.position),
            2 * Time.deltaTime);
    }

    /// <summary>
    /// This method checks if the spotted Tag is in catchable range.
    /// </summary>
    public void CheckForCatching(GameObject spotted)
    {
        var distance = Vector3.Distance(spottedPlayer.transform.position, transform.position);
        if (distance < enemyAiScriptableObject.CatchDistance)
        {
            CatchChild(spotted);
        }
    }
}