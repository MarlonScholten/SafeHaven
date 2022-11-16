using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FindHidingSpot : MonoBehaviour
{

    public LayerMask _targetMask;
    public LayerMask _obstacleMask;

    private GameObject[] _hidingSpots;

    public Transform FindClosestHidingSpot(){
        _hidingSpots = GameObject.FindGameObjectsWithTag("hideable");
        GameObject bestSpot = null;
        float bestSpotValue = -1;
        foreach(GameObject spot in _hidingSpots){
            HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
            CalculateObscurityValue(spot);           
            if(bestSpotValue == -1){
                bestSpot = spot;
                bestSpotValue = hidingSpot._obscurityValue;
            }
            else if(bestSpotValue < hidingSpot._obscurityValue){
                bestSpot = spot;
            }
        }
        // Collider[] hidingSpots = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);
        // Transform closestHidingSpot = null;         
  
        // foreach(Collider spot in hidingSpots){
        //     Transform hidingSpot = spot.transform;
        //     Vector3 dirToObject = (hidingSpot.position - transform.position).normalized;
        //         //Check if the spot is in the brothers view
        //     if(Vector3.Angle(transform.forward, dirToObject) < _viewAngle / 2){
        //         float distanceToSpot = Vector3.Distance(transform.position,hidingSpot.position);

        //         if(!Physics.Raycast(transform.position, dirToObject, distanceToSpot, _obstacleMask)){
        //                 //If there is not a current closest spot, then set one
        //             if(closestHidingSpot == null){
        //                 closestHidingSpot = hidingSpot;
        //             }
        //                 //Check if other location is closer than current closest location
        //             else if(distanceToSpot < Vector3.Distance(closestHidingSpot.transform.position, transform.position)){
        //                     closestHidingSpot = hidingSpot;
        //             }
        //         }
        //     }
        // }       
        return bestSpot.transform;
    
    }

    private void CalculateObscurityValue(GameObject spot){
        HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
        hidingSpot._obscurityValue = hidingSpot._grade + (Vector3.Distance(hidingSpot.transform.position, transform.position) * 0.05f);
        if(CheckIfSpotInEnemyFov()){
            hidingSpot._obscurityValue *= 0.01f;
        }
    }

    private bool CheckIfSpotInEnemyFov(){
        return false;
    }
}
