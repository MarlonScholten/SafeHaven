using System;
using System.Collections;
using System.Collections.Generic;
using NPC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Finite_State_Machine : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _targetWpLocation;
    public List<Transform> _wayPoints;
    private int _currentWpIndex;
    
    public float visionRange = 10f;
    public float visionAngle = 45f;
    
    
    private SoundSource _noiseMaker;
    public float threshold = 0.1f;
    private bool _alerted = false;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    /// PATROLLING ///
    public void Enter_Patrol()
    { 
        _currentWpIndex = GetClosestWaypoint();
       _targetWpLocation = _wayPoints[_currentWpIndex].position;
       _navMeshAgent.SetDestination(_targetWpLocation);
       // Play walk animation
    }
    public void Update_Patrol()
    {
        // if (checkVision())
        // {
        //     CustomEvent.Trigger(gameObject, "Alerted");
        // }
        if (_alerted)
        {
            CustomEvent.Trigger(gameObject, "Alerted");
        }
    }
    public void FixedUpdate_Patrol()
    {
        if (Vector3.Distance(transform.position, _targetWpLocation) <= 2f)
        {
            if (_navMeshAgent.isStopped == false)
            {
                _navMeshAgent.isStopped = true;
                LookAround();
            }
        }
    }
    public void Exit_Patrol()
    {
    }
    /// ALERTED ///
    public void Enter_Alerted()
    {
        
    }
    public void Update_Alerted()
    {
        if (_alerted && _noiseMaker.getVolume() > threshold)
        {
            CustomEvent.Trigger(gameObject, "Investigate");
        }
        else if(_alerted && _noiseMaker.getVolume() <= threshold)
        {
            CustomEvent.Trigger(gameObject, "Patrol");
        } else if(!_alerted)
        {
            CustomEvent.Trigger(gameObject, "Chasing");
        }
    }
    public void FixedUpdate_Alerted()
    {
    }
    
    public void Exit_Alerted()
    {
    }
    
    /// INVESTIGATE ///
    public void Enter_Investigate()
    {
        
    }
    public void Update_Investigate()
    {
    
    }
    public void FixedUpdate_Investigate()
    {
    }
    
    public void Exit_Investigate()
    {
    }
    
    /// CHASING ///
    public void Enter_Chasing()
    {
        
    }
    public void Update_Chasing()
    {
    
    }
    public void FixedUpdate_Chasing()
    {
    }
    
    public void Exit_Chasing()
    {
    }
    
    
    /// METHODS ///
    private int GetClosestWaypoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;
        for (int i = 0; i < _wayPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _wayPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }
    IEnumerator DetermineNextWaypointAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _currentWpIndex = _currentWpIndex == _wayPoints.Count - 1 ? 0 : _currentWpIndex + 1;
        _targetWpLocation = _wayPoints[_currentWpIndex].position;
        _navMeshAgent.SetDestination(_targetWpLocation);
        _navMeshAgent.isStopped = false;
        // Play walk animation
    }
    private void LookAround()
    {
        // Play look around animation
        StartCoroutine(DetermineNextWaypointAfterSeconds(3));
    }
    
    private bool checkVision()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, visionRange);
        Collider player = getPlayer(foundObjects);
        if (player != null)
        {
            //check if player is in vision angle
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle)
            {
                //check if player is in line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private Collider getPlayer(Collider[] objects)
    {
        foreach (Collider obj in objects)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                return obj;
            }
        }
        return null;
    }
    void NoiseReceived(SoundSource source)
    {
        _noiseMaker = source;
        _alerted = true;
    }
}
