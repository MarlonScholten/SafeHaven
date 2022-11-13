using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BrotherAI : MonoBehaviour
{
    [SerializeField] Transform _walkLocation;

    [SerializeField] float _runSpeed = 20;
    [SerializeField] float _walkSpeed = 10;

    private NavMeshAgent _navMeshAgent;
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        MoveToLocation(_walkLocation, _runSpeed);
    }

    private void MoveToLocation(Transform walkLocation, float speed){
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(walkLocation.position);
    }

    public void PingBrother(object action){

    }


    
}
